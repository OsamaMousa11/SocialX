using AutoMapper;
using Microsoft.Extensions.Logging;
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.Common;
using SocialX.Core.DTO.LikeDto;
using SocialX.Core.DTO.NotificationDto;
using SocialX.Core.Enumuration;
using SocialX.Core.Exceptions;
using SocialX.Core.IUnitofWork;
using SocialX.Core.ServiceContract;

namespace SocialX.Core.Service
{
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<LikeService> _logger;
        private readonly INotificationServices _notificationService;

        public LikeService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<LikeService> logger,
            INotificationServices notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _notificationService = notificationService;
        }

      
        public async Task LikeTweetAsync(Guid userId, Guid tweetId, CancellationToken ct)
        {
            var tweet = await _unitOfWork.Repository<Tweet>()
                .FindAsync(t => t.Id == tweetId && !t.IsDeleted, cancellationToken: ct);

            if (tweet == null)
                throw new NotFoundException("Tweet not found");

            var alreadyLiked = await _unitOfWork.Repository<Like>()
                .ExistsAsync(l => l.UserId == userId && l.TweetId == tweetId, ct);

            if (alreadyLiked)
                throw new ConflictException("Tweet already liked");

            var like = new Like
            {
                UserId = userId,
                TweetId = tweetId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Like>().AddAsync(like, ct);
            await _unitOfWork.CompleteAsync(ct);

           
            if (tweet.UserId != userId)
            {
                try
                {
                    await _notificationService.CreateNotificationAsync(
                        new CreateNotificationDto
                        {
                            UserId = tweet.UserId, 
                            ActorUserId = userId,   
                            Type = NotificationType.Like,
                            EntityId = tweetId,
                            Content = "liked your tweet"
                        },
                        ct);
                }
                catch { }
            }

            _logger.LogInformation(
                "User {UserId} liked tweet {TweetId}", userId, tweetId);
        }

        public async Task UnlikeTweetAsync(Guid userId, Guid tweetId, CancellationToken ct)
        {
            var like = await _unitOfWork.Repository<Like>()
                .FindAsync(l => l.UserId == userId && l.TweetId == tweetId, cancellationToken: ct);

            if (like == null)
                throw new NotFoundException("Like not found");

            _unitOfWork.Repository<Like>().Delete(like);
            await _unitOfWork.CompleteAsync(ct);

            _logger.LogInformation(
                "User {UserId} unliked tweet {TweetId}", userId, tweetId);
        }

        public async Task<bool> IsLikedTweetAsync(Guid userId, Guid tweetId, CancellationToken ct)
        {
            return await _unitOfWork.Repository<Like>()
                .ExistsAsync(l => l.UserId == userId && l.TweetId == tweetId, ct);
        }

        public async Task<PaginatedResult<LikeResponse>> GetTweetLikesAsync(
            Guid tweetId, int pageNumber, int pageSize, CancellationToken ct)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new BadRequestException("Invalid pagination parameters");

            var tweetExists = await _unitOfWork.Repository<Tweet>()
                .ExistsAsync(t => t.Id == tweetId && !t.IsDeleted, ct);

            if (!tweetExists)
                throw new NotFoundException("Tweet not found");

            var totalCount = await _unitOfWork.Repository<Like>()
                .CountAsync(l => l.TweetId == tweetId, ct);

            var likes = await _unitOfWork.Repository<Like>()
                .GetPagedAsync(
                    pageNumber,
                    pageSize,
                    l => l.TweetId == tweetId,
                    q => q.OrderByDescending(l => l.CreatedAt),
                    "User.Profile",
                    ct);

            var users = _mapper.Map<List<LikeResponse>>(likes);

            return new PaginatedResult<LikeResponse>(
                users, totalCount, pageNumber, pageSize);
        }

        public async Task<int> GetTweetLikesCountAsync(Guid tweetId, CancellationToken ct)
        {
            return await _unitOfWork.Repository<Like>()
                .CountAsync(l => l.TweetId == tweetId, ct);
        }

    

        public async Task LikeCommentAsync(Guid userId, Guid commentId, CancellationToken ct)
        {
            var comment = await _unitOfWork.Repository<Comment>()
                .FindAsync(c => c.Id == commentId && !c.IsDeleted, cancellationToken: ct);

            if (comment == null)
                throw new NotFoundException("Comment not found");

            var alreadyLiked = await _unitOfWork.Repository<Like>()
                .ExistsAsync(l => l.UserId == userId && l.CommentId == commentId, ct);

            if (alreadyLiked)
                throw new ConflictException("Comment already liked");

            var like = new Like
            {
                UserId = userId,
                CommentId = commentId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Like>().AddAsync(like, ct);
            await _unitOfWork.CompleteAsync(ct);

   
            if (comment.UserId != userId)
            {
                try
                {
                    await _notificationService.CreateNotificationAsync(
                        new CreateNotificationDto
                        {
                            UserId = comment.UserId, 
                            ActorUserId = userId,
                            Type = NotificationType.Like,
                            EntityId = commentId,
                            Content = "liked your comment"
                        },
                        ct);
                }
                catch { }
            }
        }

        public async Task UnlikeCommentAsync(Guid userId, Guid commentId, CancellationToken ct)
        {
            var like = await _unitOfWork.Repository<Like>()
                .FindAsync(l => l.UserId == userId && l.CommentId == commentId, cancellationToken: ct);

            if (like == null)
                throw new NotFoundException("Like not found");

            _unitOfWork.Repository<Like>().Delete(like);
            await _unitOfWork.CompleteAsync(ct);
        }

        public async Task<bool> IsLikedCommentAsync(Guid userId, Guid commentId, CancellationToken ct)
        {
            return await _unitOfWork.Repository<Like>()
                .ExistsAsync(l => l.UserId == userId && l.CommentId == commentId, ct);
        }

        public async Task<PaginatedResult<LikeResponse>> GetCommentLikesAsync(
            Guid commentId, int pageNumber, int pageSize, CancellationToken ct)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new BadRequestException("Invalid pagination parameters");

            var commentExists = await _unitOfWork.Repository<Comment>()
                .ExistsAsync(c => c.Id == commentId && !c.IsDeleted, ct);

            if (!commentExists)
                throw new NotFoundException("Comment not found");

            var totalCount = await _unitOfWork.Repository<Like>()
                .CountAsync(l => l.CommentId == commentId, ct);

            var likes = await _unitOfWork.Repository<Like>()
                .GetPagedAsync(
                    pageNumber,
                    pageSize,
                    l => l.CommentId == commentId,
                    q => q.OrderByDescending(l => l.CreatedAt),
                    "User.Profile",
                    ct);

            var users = _mapper.Map<List<LikeResponse>>(likes);

            return new PaginatedResult<LikeResponse>(
                users, totalCount, pageNumber, pageSize);
        }

        public async Task<int> GetCommentLikesCountAsync(Guid commentId, CancellationToken ct)
        {
            return await _unitOfWork.Repository<Like>()
                .CountAsync(l => l.CommentId == commentId, ct);
        }
    }
}
