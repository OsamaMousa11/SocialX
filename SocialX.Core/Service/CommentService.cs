using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.CommentDto;
using SocialX.Core.DTO.Common;
using SocialX.Core.DTO.NotificationDto;
using SocialX.Core.Enumuration;
using SocialX.Core.Exceptions;
using SocialX.Core.IUnitofWork;
using SocialX.Core.ServiceContract;

namespace SocialX.Core.Service
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IMentionService _mentionService;
        private readonly ILogger<CommentService> _logger;
        private readonly INotificationServices _notificationService;

        public CommentService(
       IUnitOfWork unitOfWork,
       IMapper mapper,
       IFileService fileService,
       IMentionService mentionService,
       ILogger<CommentService> logger,
       INotificationServices notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _mentionService = mentionService;
            _logger = logger;
            _notificationService = notificationService;
        }


        // ===================== ADD =====================
        public async Task<CommentResponse> AddCommentAsync(
            Guid userId,
            CommentAddRequest request,
            CancellationToken cancellationToken = default)
        {
            var tweetExists = await _unitOfWork.Repository<Tweet>()
                .ExistsAsync(t => t.Id == request.TweetId && !t.IsDeleted, cancellationToken);

            if (!tweetExists)
                throw new NotFoundException("Tweet not found");


            if (request.ParentCommentId.HasValue)
            {
                var parentExists = await _unitOfWork.Repository<Comment>()
                    .ExistsAsync(c => c.Id == request.ParentCommentId && !c.IsDeleted, cancellationToken);

                if (!parentExists)
                    throw new NotFoundException("Parent comment not found");
            }

            var comment = _mapper.Map<Comment>(request);
            comment.UserId = userId;
            comment.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Comment>()
                .AddAsync(comment, cancellationToken);

            // Attachments
            if (request.AttachmentFiles?.Any() == true)
            {
                foreach (var file in request.AttachmentFiles)
                {
                    var url = await _fileService.UploadFileAsync(file, "comments", cancellationToken);

                    await _unitOfWork.Repository<Attachment>().AddAsync(
                        new Attachment
                        {
                            CommentId = comment.Id,
                            FileUrl = url,
                            FileSize = file.Length
                        }, cancellationToken);
                }
            }

            // Mentions
            /* if (request.MentionedUserIds?.Any() == true)
             {
                 await _mentionService.CreateMentionsFromContentAsync(
                     request.MentionedUserId()
             */

            await _unitOfWork.CompleteAsync(cancellationToken);

         
            var tweet = await _unitOfWork.Repository<Tweet>()
                .GetByIdAsync(request.TweetId);

            if (tweet.UserId != userId)
            {
                await _notificationService.CreateNotificationAsync(
                    new CreateNotificationDto
                    {
                        UserId = tweet.UserId,     
                        ActorUserId = userId,       
                        Type = NotificationType.Comment,
                        EntityId = comment.Id,
                        Content = "commented on your tweet"
                    },
                    cancellationToken
                );
            }

            _logger.LogInformation(
                "User {UserId} added comment {CommentId}",
                userId,
                comment.Id
            );

            return _mapper.Map<CommentResponse>(comment);

        }

        public async Task<PaginatedResult<CommentResponse>> GetCommentsByTweetIdAsync(
    Guid? currentUserId,
    Guid tweetId,
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken = default)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new BadRequestException("Invalid pagination parameters");

            var tweetExists = await _unitOfWork.Repository<Tweet>()
                .ExistsAsync(t => t.Id == tweetId && !t.IsDeleted, cancellationToken);

            if (!tweetExists)
                throw new NotFoundException("Tweet not found");

            var totalCount = await _unitOfWork.Repository<Comment>()
                .CountAsync(c => c.TweetId == tweetId && !c.IsDeleted, cancellationToken);

            var comments = await _unitOfWork.Repository<Comment>()
                .GetPagedAsync(
                    pageNumber,
                    pageSize,
                    predicate: c => c.TweetId == tweetId && !c.IsDeleted,
                    orderBy: q => q.OrderByDescending(c => c.CreatedAt),
                    includeProperties: "User.Profile,Attachments,Likes,Replies",
                    cancellationToken: cancellationToken);

            var responses = _mapper.Map<List<CommentResponse>>(comments);


            if (currentUserId.HasValue)
            {
                foreach (var comment in comments)
                {
                    var response = responses.First(r => r.Id == comment.Id);
                    response.IsLikedByCurrentUser =
                        comment.Likes.Any(l => l.UserId == currentUserId.Value);
                }
            }

            return new PaginatedResult<CommentResponse>(
                responses,
                totalCount,
                pageNumber,
                pageSize);
        }


        // ===================== GET BY ID =====================
        public async Task<CommentResponse> GetCommentByIdAsync(
            Guid? currentUserId,
            Guid commentId,
            CancellationToken cancellationToken = default)
        {
            var comment = await _unitOfWork.Repository<Comment>()
                .FindAsync(
                    c => c.Id == commentId && !c.IsDeleted,
                    includeProperties: "User.Profile,Attachments,Likes,Replies,Mentions",
                    cancellationToken: cancellationToken);

            if (comment == null)
                throw new NotFoundException("Comment not found");

            var response = _mapper.Map<CommentResponse>(comment);

            response.IsLikedByCurrentUser =
                currentUserId.HasValue &&
                comment.Likes.Any(l => l.UserId == currentUserId.Value);

            return response;
        }

        // ===================== DELETE =====================
        public async Task DeleteCommentAsync(
            Guid userId,
            Guid commentId,
            CancellationToken cancellationToken = default)
        {
            var comment = await _unitOfWork.Repository<Comment>()
                .FindAsync(c => c.Id == commentId && !c.IsDeleted, cancellationToken: cancellationToken);

            if (comment == null)
                throw new NotFoundException("Comment not found");

            if (comment.UserId != userId)
                throw new ForbiddenException("You are not allowed to delete this comment");

            comment.IsDeleted = true;
            _unitOfWork.Repository<Comment>().Update(comment);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        public async Task<CommentResponse> UpdateCommentAsync(
      Guid userId,
      Guid commentId,
      CommentUpdateRequest request,
      CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
                throw new BadRequestException("Content cannot be empty");

            var comment = await _unitOfWork.Repository<Comment>()
                .FindAsync(c => c.Id == commentId && !c.IsDeleted,
                           includeProperties: "User.Profile,Attachments,Likes,Replies",
                           cancellationToken: cancellationToken);

            if (comment == null)
                throw new NotFoundException("Comment not found");

            if (comment.UserId != userId)
                throw new ForbiddenException("You are not allowed to edit this comment");

            comment.Content = request.Content;
            comment.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Comment>().Update(comment);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return _mapper.Map<CommentResponse>(comment);
        }

    }
}
