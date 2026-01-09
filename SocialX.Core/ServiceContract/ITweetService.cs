
using SocialX.Core.DTO.Common;
using SocialX.Core.DTO.TweetDto;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocialX.Core.ServiceContract
{
    public interface ITweetService
    {
        Task<ApiResponse<TweetResponse>> AddTweetAsync(
            Guid userId,
            TweetAddRequest request,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<TweetResponse>> UpdateTweetAsync(
            Guid userId,
            Guid tweetId,
            TweetUpdateRequest request,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> DeleteTweetAsync(
            Guid userId,
            Guid tweetId,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<TweetResponse>> GetTweetByIdAsync(
            Guid? userId,
            Guid tweetId,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<PaginatedResult<TweetResponse>>> GetFeedAsync(
            Guid userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> LikeTweetAsync(
            Guid userId,
            Guid tweetId,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> UnlikeTweetAsync(
            Guid userId,
            Guid tweetId,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> BookmarkTweetAsync(
            Guid userId,
            Guid tweetId,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> UnbookmarkTweetAsync(
            Guid userId,
            Guid tweetId,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<TweetResponse>> RetweetAsync(
            Guid userId,
            Guid tweetId,
            CancellationToken cancellationToken = default);
    }
}