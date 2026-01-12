using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialX.Api.Extensions;
using SocialX.Core.ServiceContract;

namespace SocialX.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/tweets")]
    public class BookmarkController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;

        public BookmarkController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        [HttpPost("{tweetId}/bookmark")]
        public async Task<IActionResult> BookmarkAsync(
            Guid tweetId,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            await _bookmarkService.BookmarkAsync(
                userId,
                tweetId,
                cancellationToken);

            return Ok();
        }

        [HttpDelete("{tweetId}/bookmark")]
        public async Task<IActionResult> UnbookmarkAsync(
            Guid tweetId,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            await _bookmarkService.UnbookmarkAsync(
                userId,
                tweetId,
                cancellationToken);

            return Ok();
        }

        [HttpGet("{tweetId}/bookmark")]
        public async Task<IActionResult> IsBookmarkedAsync(
            Guid tweetId,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var isBookmarked = await _bookmarkService.IsBookmarkedAsync(
                userId,
                tweetId,
                cancellationToken);

            return Ok(isBookmarked);
        }

        [HttpGet("me/bookmarks")]
        public async Task<IActionResult> GetMyBookmarks(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var userId = User.GetUserId();

            var result = await _bookmarkService.GetUserBookmarksAsync(
                userId,
                pageNumber,
                pageSize,
                cancellationToken);

            return Ok(result);
        }
    }
}
