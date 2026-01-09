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
        Task<ApiResponse<bool>> LikeTweetAsync(Guid userId, Guid tweetId, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> UnlikeTweetAsync(Guid userId, Guid tweetId, CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> LikeCommentAsync(Guid userId, Guid commentId, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> UnlikeCommentAsync(Guid userId, Guid commentId, CancellationToken cancellationToken = default);


        Task<bool> IsLikedTweetAsync(Guid userId, Guid tweetId, CancellationToken cancellationToken = default);
        Task<bool> IsLikedCommentAsync(Guid userId, Guid commentId, CancellationToken cancellationToken = default);

        Task<ApiResponse<PaginatedResult<LikedUserResponse>>> GetTweetLikesAsync(
    Guid tweetId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        Task<ApiResponse<PaginatedResult<LikedUserResponse>>> GetCommentLikesAsync(
            Guid commentId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        Task<ApiResponse<int>> GetTweetLikesCountAsync(
            Guid tweetId,
            CancellationToken cancellationToken = default);
    Task<ApiResponse<int>> GetCommentLikesCountAsync(
           Guid commentId,
           CancellationToken cancellationToken = default);


    }
}
