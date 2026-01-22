using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialX.Api.Extensions;
using SocialX.Core.DTO.Common;
using SocialX.Core.DTO.TweetDto;
using SocialX.Core.ServiceContract;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SocialX.Api.Controllers
{
    [ApiController]
    [Route("api/tweets")]
    [Authorize] // كل الـ endpoints محتاجة login
    public class TweetsController : ControllerBase
    {
        private readonly ITweetService _tweetService;

        public TweetsController(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }

        
        [HttpPost]
        [Consumes("multipart/form-data")] 
        public async Task<IActionResult> CreateTweet(
            [FromForm] TweetAddRequest request,
            CancellationToken cancellationToken = default)
        {
            var userId = User.GetUserId(); 

            var result = await _tweetService.AddTweetAsync(userId, request, cancellationToken);

            return Ok(result); // using ApiResponse extension
        }

       
        [HttpPut("{tweetId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateTweet(
            Guid tweetId,
            [FromForm] TweetUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            var userId = User.GetUserId();

            var result = await _tweetService.UpdateTweetAsync(userId, tweetId, request, cancellationToken);

              return Ok(result);
        }

        /// <summary>
        /// Delete a tweet (soft delete)
        /// </summary>
        [HttpDelete("{tweetId}")]
        public async Task<IActionResult> DeleteTweet(
            Guid tweetId,
            CancellationToken cancellationToken = default)
        {
            var userId = User.GetUserId();

            var success = await _tweetService.DeleteTweetAsync(userId, tweetId, cancellationToken);

            if (!success)
                return NotFound(ApiResponse<string>.FailureResponse("Tweet not found or not authorized", HttpStatusCode.NotFound));

            return NoContent();
        }

        /// <summary>
        /// Get a single tweet by ID (with personalization: isLiked, isBookmarked...)
        /// </summary>
        [HttpGet("{tweetId}")]
        [AllowAnonymous] // optional: allow viewing without login
        public async Task<IActionResult> GetTweet(
            Guid tweetId,
            CancellationToken cancellationToken = default)
        {
            // Guid? currentUserId = User.IsAuthenticated() ? User.GetUserId() : null;
            Guid? currentUserId =  User.GetUserId()  ;

            var result = await _tweetService.GetTweetByIdAsync(currentUserId, tweetId, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get user feed/timeline (tweets from followed users + own tweets)
        /// </summary>
        [HttpGet("feed")]
        public async Task<IActionResult> GetFeed(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var userId = User.GetUserId();

            var result = await _tweetService.GetFeedAsync(userId, pageNumber, pageSize, cancellationToken);

            return Ok(result);
        }
    }
}