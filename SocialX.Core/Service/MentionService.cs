using AutoMapper;
using Microsoft.Extensions.Logging;
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.Common;
using SocialX.Core.DTO.MentionDto;
using SocialX.Core.IUnitofWork;
using SocialX.Core.ServiceContract;
using System.Linq;

namespace SocialX.Application.Services
{
    public class MentionService : IMentionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MentionService> _logger;

        public MentionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PaginatedResult<MentionResponse>>> GetUserMentionsAsync(
              Guid userId,
              int pageNumber,
              int pageSize,
              CancellationToken cancellationToken = default)
        {
            var mentions = await _unitOfWork.Repository<Mention>()
                .FindAllAsync(
                    m => m.MentionedUserId == userId,
                    includeProperties: "Tweet.User.Profile",
                    cancellationToken: cancellationToken);

            var validMentions = mentions
                .Where(m => m.Tweet != null && !m.Tweet.IsDeleted)
                .OrderByDescending(m => m.CreatedAt)
                .ToList();

            var totalCount = validMentions.Count;

            var pagedMentions = validMentions
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (!pagedMentions.Any())
            {
                return ApiResponse<PaginatedResult<MentionResponse>>.SuccessResponse(
                    new PaginatedResult<MentionResponse>(
                        new List<MentionResponse>(),
                        totalCount,
                        pageNumber,
                        pageSize));
            }

            var tweetIds = pagedMentions.Select(m => m.TweetId).Distinct().ToList();



            var likes = await _unitOfWork.Repository<Like>()
                .FindAllAsync(l => tweetIds.Contains(l.Id), cancellationToken: cancellationToken);

            var comments = await _unitOfWork.Repository<Comment>()
                .FindAllAsync(c => tweetIds.Contains(c.TweetId) && !c.IsDeleted, cancellationToken: cancellationToken);

            var retweets = await _unitOfWork.Repository<Tweet>()
                .FindAllAsync(t => t.OriginalTweetId != null && tweetIds.Contains(t.OriginalTweetId.Value),
                    cancellationToken: cancellationToken);

            var userLikes = await _unitOfWork.Repository<Like>()
                .FindAllAsync(l => l.UserId == userId && tweetIds.Contains(l.Id),
                    cancellationToken: cancellationToken);

            var userBookmarks = await _unitOfWork.Repository<Bookmark>()
                .FindAllAsync(b => b.UserId == userId && tweetIds.Contains(b.TweetId),
                    cancellationToken: cancellationToken);

            var likesDict = likes.GroupBy(l => l.TweetId).ToDictionary(g => g.Key, g => g.Count());
            var commentsDict = comments.GroupBy(c => c.TweetId).ToDictionary(g => g.Key, g => g.Count());
            var retweetsDict = retweets.GroupBy(r => r.OriginalTweetId!.Value).ToDictionary(g => g.Key, g => g.Count());

            var likedSet = userLikes.Select(l => l.TweetId).ToHashSet();
            var bookmarkedSet = userBookmarks.Select(b => b.TweetId).ToHashSet();

            var result = pagedMentions.Select(m =>
            {
                var dto = _mapper.Map<MentionResponse>(m.Tweet);

                dto.LikesCount = likesDict.GetValueOrDefault(m.TweetId, 0);
                dto.CommentsCount = commentsDict.GetValueOrDefault(m.TweetId, 0);
                dto.RetweetsCount = retweetsDict.GetValueOrDefault(m.TweetId, 0);
                dto.IsLikedByCurrentUser = likedSet.Contains(m.TweetId);
                dto.IsBookmarkedByCurrentUser = bookmarkedSet.Contains(m.TweetId);

                return dto;
            }).ToList();

            return ApiResponse<PaginatedResult<MentionResponse>>.SuccessResponse(
                new PaginatedResult<MentionResponse>(
                    result,
                    totalCount,
                    pageNumber,
                    pageSize));
        }
    }
}
