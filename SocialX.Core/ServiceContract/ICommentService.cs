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

    
            Task<CommentResponse> GetCommentByIdAsync(
                Guid? currentUserId,
                Guid commentId,
                CancellationToken cancellationToken = default);

    
                    Task<PaginatedResult<CommentResponse>> GetCommentsByTweetIdAsync(
                Guid? currentUserId,
                Guid tweetId,
                int pageNumber,
                int pageSize,
                CancellationToken cancellationToken = default);

        Task<CommentResponse> UpdateCommentAsync(
    Guid userId,
    Guid commentId,
    CommentUpdateRequest request,
    CancellationToken cancellationToken = default);


        Task DeleteCommentAsync(
            Guid userId,
            Guid commentId,
            CancellationToken cancellationToken = default);
    }
}
