using SocialX.Core.DTO.CommentDto;
using SocialX.Core.DTO.Common;

namespace SocialX.Core.ServiceContract
{
    public interface ICommentService
    {
        /// <summary>
        /// Add new comment to a tweet (or reply to another comment)
        /// </summary>
        Task<CommentResponse> AddCommentAsync(
            Guid userId,
            CommentAddRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get comment by id (with attachments, mentions, likes, replies count)
        /// </summary>
        Task<CommentResponse> GetCommentByIdAsync(
            Guid? currentUserId,
            Guid commentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete comment (soft delete)
        /// </summary>
        Task DeleteCommentAsync(
            Guid userId,
            Guid commentId,
            CancellationToken cancellationToken = default);
    }
}
