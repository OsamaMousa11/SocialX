using SocialX.Core.DTO.Common;
using SocialX.Core.DTO.LikeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.ServiceContract
{
    public interface ILikeService
    {
        Task LikeTweetAsync(Guid userId, Guid tweetId, CancellationToken cancellationToken = default);
        Task UnlikeTweetAsync(Guid userId, Guid tweetId, CancellationToken cancellationToken = default);

        Task LikeCommentAsync(Guid userId, Guid commentId, CancellationToken cancellationToken = default);
        Task UnlikeCommentAsync(Guid userId, Guid commentId, CancellationToken cancellationToken = default);

        Task<bool> IsLikedTweetAsync(Guid userId, Guid tweetId, CancellationToken cancellationToken = default);
        Task<bool> IsLikedCommentAsync(Guid userId, Guid commentId, CancellationToken cancellationToken = default);

        Task<PaginatedResult<LikeResponse>> GetTweetLikesAsync(
            Guid tweetId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        Task<PaginatedResult<LikeResponse>> GetCommentLikesAsync(
            Guid commentId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        Task<int> GetTweetLikesCountAsync(Guid tweetId, CancellationToken cancellationToken = default);
        Task<int> GetCommentLikesCountAsync(Guid commentId, CancellationToken cancellationToken = default);
    }

}
