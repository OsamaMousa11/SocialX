using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialX.Core.DTO.CommentDto;
using SocialX.Core.DTO.Common;
using SocialX.Core.ServiceContract;
using System.Security.Claims;

namespace SocialX.API.Controllers
{
    [ApiController]
    [Route("api/comments")]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

      
        [HttpGet("tweet/{tweetId}")]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResult<CommentResponse>>> GetTweetComments(
            Guid tweetId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var userId = GetCurrentUserId();

            var result = await _commentService.GetTweetCommentsAsync(
                userId, tweetId, pageNumber, pageSize, cancellationToken);

            return Ok(result);
        }

       
        [HttpGet("{commentId}/replies")]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResult<CommentResponse>>> GetCommentReplies(
            Guid commentId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var userId = GetCurrentUserId();

            var result = await _commentService.GetCommentRepliesAsync(
                userId, commentId, pageNumber, pageSize, cancellationToken);

            return Ok(result);
        }

        
        [HttpGet("{commentId}")]
        [AllowAnonymous]
        public async Task<ActionResult<CommentDetailsResponse>> GetCommentById(
            Guid commentId,
            CancellationToken cancellationToken = default)
        {
            var userId = GetCurrentUserId();

            var comment = await _commentService.GetCommentByIdAsync(
                userId, commentId, cancellationToken);

            return Ok(comment);
        }

       
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<CommentResponse>> AddComment(
            [FromForm] CommentAddRequest request,
            CancellationToken cancellationToken = default)
        {
            var userId = GetCurrentUserIdOrThrow();

            var comment = await _commentService.AddCommentAsync(
                userId, request, cancellationToken);

            return CreatedAtAction(
                nameof(GetCommentById),
                new { commentId = comment.Id },
                comment);
        }

       
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(
            Guid commentId,
            CancellationToken cancellationToken = default)
        {
            var userId = GetCurrentUserIdOrThrow();

            await _commentService.DeleteCommentAsync(
                userId, commentId, cancellationToken);

            return NoContent();
        }


        private Guid? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
                return userId;

            return null;
        }

        private Guid GetCurrentUserIdOrThrow()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
                throw new UnauthorizedAccessException("User is not authenticated");

            return userId.Value;
        }
    }
}
