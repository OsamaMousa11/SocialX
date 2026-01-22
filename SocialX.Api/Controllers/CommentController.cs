using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialX.Api.Extensions;
using SocialX.Core.DTO.CommentDto;
using SocialX.Core.ServiceContract;

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

        // POST
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<CommentResponse>> Add(
            [FromForm] CommentAddRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var result = await _commentService.AddCommentAsync(
                userId, request, cancellationToken);

            return Ok(result);
        }

        // GET BY ID
        [HttpGet("{commentId}")]
        [AllowAnonymous]
        public async Task<ActionResult<CommentResponse>> GetById(
            Guid commentId,
            CancellationToken cancellationToken)
        {
            Guid? userId = User.Identity?.IsAuthenticated == true
                ? User.GetUserId()
                : null;

            var result = await _commentService.GetCommentByIdAsync(
                userId, commentId, cancellationToken);

            return Ok(result);
        }

        // DELETE
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> Delete(
            Guid commentId,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            await _commentService.DeleteCommentAsync(
                userId, commentId, cancellationToken);

            return NoContent();
        }
    }
}
