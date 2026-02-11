
using SocialX.Core.DTO.Common;
using SocialX.Core.DTO.TweetDto;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocialX.Core.ServiceContract
{
    public interface ITweetService
    {
        Task<TweetResponse> AddTweetAsync(
           Guid userId,
           TweetAddRequest request,
           CancellationToken cancellationToken = default);

        Task<TweetResponse> UpdateTweetAsync(
            Guid userId,
            Guid tweetId,
            TweetUpdateRequest request,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteTweetAsync(
            Guid userId,
            Guid tweetId,
            CancellationToken cancellationToken = default);

        Task<TweetResponse> GetTweetByIdAsync(
            Guid? userId,
            Guid tweetId,
            CancellationToken cancellationToken = default);


        Task<PaginatedResult<TweetResponse>> GetFeedAsync(
          Guid userId,
          int pageNumber,
          int pageSize,
          CancellationToken cancellationToken = default);

        Task<PaginatedResult<TweetResponse>> GetTweetsByUserIdAsync(
    Guid userId,
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken = default);

    }
}