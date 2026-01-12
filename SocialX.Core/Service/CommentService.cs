using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.CommentDto;
using SocialX.Core.DTO.Common;
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

        public CommentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService,
            IMentionService mentionService,
            ILogger<CommentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _mentionService = mentionService;
            _logger = logger;
        }

        public async Task<CommentResponse> AddCommentAsync(Guid userId,CommentAddRequest request,CancellationToken cancellationToken = default)
        {
            ValidateUserId(userId);

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var tweetExists = await _unitOfWork.Repository<Tweet>()
                .ExistsAsync(t => t.Id == request.TweetId && !t.IsDeleted, cancellationToken);

            if (!tweetExists)
                throw new NotFoundException("Tweet not found");

            if (request.ParentCommentId.HasValue)
            {
                var parentExists = await _unitOfWork.Repository<Comment>()
                    .ExistsAsync(
                        c => c.Id == request.ParentCommentId.Value && !c.IsDeleted,
                        cancellationToken);

                if (!parentExists)
                    throw new NotFoundException("Parent comment not found");
            }

            var uploadedAttachments = await UploadAttachmentsAsync(
                request.AttachmentFiles, cancellationToken);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var comment = _mapper.Map<Comment>(request);
                comment.UserId = userId;
                comment.CreatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Comment>()
                    .AddAsync(comment, cancellationToken);

                await _mentionService.CreateMentionsFromContentAsync(
                    comment.Id,
                    request.Content,
                    isTweet: false,
                    cancellationToken);

                foreach (var attachment in uploadedAttachments)
                {
                    attachment.CommentId = comment.Id;
                    await _unitOfWork.Repository<Attachment>()
                        .AddAsync(attachment, cancellationToken);
                }

                await _unitOfWork.CompleteAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation(
                    "User {UserId} created comment {CommentId} on tweet {TweetId}",
                    userId, comment.Id, request.TweetId);

                return await MapCommentToResponseAsync(comment.Id, userId, cancellationToken);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _logger.LogError(ex, "Error adding comment for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> DeleteCommentAsync(
            Guid userId,
            Guid commentId,
            CancellationToken cancellationToken = default)
        {
            ValidateUserId(userId);
            ValidateGuid(commentId, nameof(commentId));

            var comment = await _unitOfWork.Repository<Comment>()
                .FindAsync(
                    c => c.Id == commentId && !c.IsDeleted,
                    includeProperties: "Attachments,Replies",
                    cancellationToken: cancellationToken);

            if (comment == null)
                throw new NotFoundException("Comment not found");

            if (comment.UserId != userId)
                throw new UnauthorizedException("You are not authorized to delete this comment");

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                if (comment.Attachments?.Any() == true)
                {
                    var fileDeletionTasks = comment.Attachments
                        .Select(a => _fileService.DeleteFileAsync(a.FileUrl, cancellationToken))
                        .ToList();

                    await Task.WhenAll(fileDeletionTasks);
                }

                if (comment.Replies?.Any() == true)
                {
                    foreach (var reply in comment.Replies.Where(r => !r.IsDeleted))
                    {
                        reply.IsDeleted = true;
                        _unitOfWork.Repository<Comment>().Update(reply);
                    }
                }

                comment.IsDeleted = true;
                _unitOfWork.Repository<Comment>().Update(comment);

                await _unitOfWork.CompleteAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation(
                    "User {UserId} deleted comment {CommentId} with {RepliesCount} replies",
                    userId, commentId, comment.Replies?.Count ?? 0);

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _logger.LogError(ex, "Error deleting comment {CommentId} for user {UserId}",
                    commentId, userId);
                throw;
            }
        }

        public async Task<CommentDetailsResponse> GetCommentByIdAsync(
            Guid? userId,
            Guid commentId,
            CancellationToken cancellationToken = default)
        {
            ValidateGuid(commentId, nameof(commentId));

            var comment = await _unitOfWork.Repository<Comment>()
                .FindAsync(
                    c => c.Id == commentId && !c.IsDeleted,
                    includeProperties: "User.Profile,Attachments,Replies",
                    cancellationToken: cancellationToken);

            if (comment == null)
                throw new NotFoundException("Comment not found");

            return _mapper.Map<CommentDetailsResponse>(comment);
        }

        public async Task<PaginatedResult<CommentResponse>> GetCommentRepliesAsync(
            Guid? userId,
            Guid commentId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            ValidateGuid(commentId, nameof(commentId));
            ValidatePaginationParameters(pageNumber, pageSize);

            var parentExists = await _unitOfWork.Repository<Comment>()
                .ExistsAsync(c => c.Id == commentId && !c.IsDeleted, cancellationToken);

            if (!parentExists)
                throw new NotFoundException("Comment not found");

            var totalCount = await _unitOfWork.Repository<Comment>()
                .CountAsync(c => c.ParentCommentId == commentId && !c.IsDeleted, cancellationToken);

            var replies = await _unitOfWork.Repository<Comment>()
                .GetPagedAsync(
                    pageNumber,
                    pageSize,
                    predicate: c => c.ParentCommentId == commentId && !c.IsDeleted,
                    orderBy: q => q.OrderByDescending(c => c.CreatedAt),
                    includeProperties: "User.Profile,Attachments",
                    cancellationToken: cancellationToken);

            var responses = _mapper.Map<List<CommentResponse>>(replies);

            return new PaginatedResult<CommentResponse>(
                responses,
                totalCount,
                pageNumber,
                pageSize);
        }

        public async Task<PaginatedResult<CommentResponse>> GetTweetCommentsAsync(
            Guid? userId,
            Guid tweetId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            ValidateGuid(tweetId, nameof(tweetId));
            ValidatePaginationParameters(pageNumber, pageSize);

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
                    includeProperties: "User.Profile,Attachments",
                    cancellationToken: cancellationToken);

            var responses = _mapper.Map<List<CommentResponse>>(comments);

            return new PaginatedResult<CommentResponse>(
                responses,
                totalCount,
                pageNumber,
                pageSize);
        }

        private MediaType DetermineAttachmentType(string contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType))
                return MediaType.File;

            if (contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                return MediaType.Image;

            if (contentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))
                return MediaType.Video;

            return MediaType.File;
        }

        private async Task<List<Attachment>> UploadAttachmentsAsync(
            IEnumerable<IFormFile>? files,
            CancellationToken cancellationToken)
        {
            var attachments = new List<Attachment>();

            if (files?.Any() != true)
                return attachments;

            foreach (var file in files)
            {
                try
                {
                    var fileUrl = await _fileService.UploadFileAsync(
                        file, "comments", cancellationToken);

                    attachments.Add(new Attachment
                    {
                        FileUrl = fileUrl,
                        FileSize = file.Length,
                        Type = DetermineAttachmentType(file.ContentType)
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error uploading file: {FileName}", file.FileName);

                    foreach (var attachment in attachments)
                    {
                        await _fileService.DeleteFileAsync(attachment.FileUrl, cancellationToken);
                    }
                    throw;
                }
            }

            return attachments;
        }

        private void ValidatePaginationParameters(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentException("Page number must be greater than 0", nameof(pageNumber));

            if (pageSize < 1)
                throw new ArgumentException("Page size must be greater than 0", nameof(pageSize));

            if (pageSize > 100)
                throw new ArgumentException("Page size cannot exceed 100", nameof(pageSize));
        }

        private void ValidateUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty", nameof(userId));
        }

        private void ValidateGuid(Guid id, string paramName)
        {
            if (id == Guid.Empty)
                throw new ArgumentException($"{paramName} cannot be empty", paramName);
        }

        private async Task<CommentResponse> MapCommentToResponseAsync(Guid commentId,Guid? userId,CancellationToken cancellationToken)
        {
            var comment = await _unitOfWork.Repository<Comment>()
                .FindAsync(
                    c => c.Id == commentId && !c.IsDeleted,
                    includeProperties: "User.Profile,Attachments",
                    cancellationToken: cancellationToken);

            return _mapper.Map<CommentResponse>(comment);
        }
    }
}