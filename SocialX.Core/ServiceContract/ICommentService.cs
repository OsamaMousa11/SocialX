using SocialX.Core.DTO.CommentDto;
using SocialX.Core.DTO.Common;

namespace SocialX.Core.ServiceContract
{
    public interface ICommentService
    {
        Task<CommentResponse> AddCommentAsync(
            Guid userId,
            CommentAddRequest request,
            CancellationToken cancellationToken = default);

        Task<bool>DeleteCommentAsync(
            Guid userId,
            Guid commentId,
            CancellationToken cancellationToken = default);

        Task<CommentDetailsResponse> GetCommentByIdAsync(
            Guid? userId,
            Guid commentId,
            CancellationToken cancellationToken = default);

        Task<PaginatedResult<CommentResponse>> GetTweetCommentsAsync(
            Guid? userId,
            Guid tweetId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<PaginatedResult<CommentResponse>> GetCommentRepliesAsync(
            Guid? userId,
            Guid commentId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
    }

}