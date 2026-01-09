using Microsoft.Extensions.Logging;
using SocialX.Core.Domain.Entites;
using SocialX.Core.Domain.IRepositoryContract;
using SocialX.Core.DTO.Common;
using SocialX.Core.DTO.TweetDto;
using SocialX.Core.IUnitofWork;
using SocialX.Core.ServiceContract;


namespace SocialX.Core.Service
{
    public class BookMarkService : IBookmarkService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<BookMarkService> _logger;


        public BookMarkService(IUnitOfWork unitOfWork,

            ILogger<BookMarkService> logger)
        {
            _unitOfWork = unitOfWork;

            _logger = logger;
        }

        public async Task<ApiResponse<bool>> BookmarkAsync(Guid userId, Guid tweetId, CancellationToken cancellationToken = default)
        {

            var tweetExists = await _unitOfWork.Repository<Tweet>().ExistsAsync(
                t => t.Id == tweetId,
                cancellationToken);

            if (!tweetExists)
                return ApiResponse<bool>.FailureResponse("Tweet not found");

            var alreadyBookmarked = await _unitOfWork.Repository<Bookmark>().ExistsAsync(
                b => b.UserId == userId && b.TweetId == tweetId,
                cancellationToken);

            if (alreadyBookmarked)
                return ApiResponse<bool>.FailureResponse("Tweet already bookmarked");


            var bookmark = new Bookmark
            {

                UserId = userId,
                TweetId = tweetId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Bookmark>().AddAsync(bookmark, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<bool> IsBookmarkedAsync(Guid userId, Guid tweetId, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<Bookmark>().ExistsAsync(
                 b => b.UserId == userId && b.TweetId == tweetId,
                 cancellationToken);

        }

        public async Task<ApiResponse<bool>> UnbookmarkAsync(Guid userId, Guid tweetId, CancellationToken cancellationToken = default)
        {

            var bookmark = await _unitOfWork.Repository<Bookmark>().FindAsync(
           b => b.UserId == userId && b.TweetId == tweetId,
           cancellationToken: cancellationToken);

            if (bookmark == null)
                return ApiResponse<bool>.FailureResponse("Bookmark not found");

            _unitOfWork.Repository<Bookmark>().Delete(bookmark);
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("User {UserId} unbookmarked tweet {TweetId}", userId, tweetId);

            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<ApiResponse<PaginatedResult<TweetResponse>>> GetUserBookmarksAsync(
    Guid userId,
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken = default)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return ApiResponse<PaginatedResult<TweetResponse>>
                    .FailureResponse("Invalid pagination parameters");

           
            var totalCount = await _unitOfWork.Repository<Bookmark>()
                .CountAsync(b => b.UserId == userId, cancellationToken);


         var bookmarks = await _unitOfWork.Repository<Bookmark>()
                .FindAllAsync(
                    b => b.UserId == userId,
                    includeProperties: "Tweet, Tweet.User",
                    cancellationToken: cancellationToken);

            var pagedBookmarks = bookmarks
                .OrderByDescending(b => b.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

     
            var tweets = pagedBookmarks.Select(b => new TweetResponse
            {
                Id = b.Tweet.Id,
                Content = b.Tweet.Content,
                CreatedAt = b.Tweet.CreatedAt,
                UpdatedAt = b.Tweet.UpdatedAt,

                UserId = b.Tweet.UserId,
                UserName = b.Tweet.User.UserName,


            }).ToList();

           
            var paginatedResult = new PaginatedResult<TweetResponse>(
                tweets,
                totalCount,
                pageNumber,
                pageSize);

            return ApiResponse<PaginatedResult<TweetResponse>>
                .SuccessResponse(paginatedResult);
        }

    }
}

