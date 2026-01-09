using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialX.Api.Extensions;
using SocialX.Core.ServiceContract;

namespace SocialX.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api")]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

      

        // POST: api/tweets/{tweetId}/like
        [HttpPost("tweets/{tweetId}/like")]
        public async Task<IActionResult> LikeTweet(
            Guid tweetId,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _likeService.LikeTweetAsync(userId, tweetId, cancellationToken);
            return Ok(result);
        }

 
        [HttpDelete("tweets/{tweetId}/like")]
        public async Task<IActionResult> UnlikeTweet(
            Guid tweetId,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _likeService.UnlikeTweetAsync(userId, tweetId, cancellationToken);
            return Ok(result);
        }

      
        [HttpGet("tweets/{tweetId}/like")]
        public async Task<IActionResult> IsLikedTweet(
            Guid tweetId,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var isLiked = await _likeService.IsLikedTweetAsync(userId, tweetId, cancellationToken);
            return Ok(new { isLiked });
        }

  
        [HttpGet("tweets/{tweetId}/likes")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTweetLikes(
            Guid tweetId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var result = await _likeService.GetTweetLikesAsync(
                tweetId, pageNumber, pageSize, cancellationToken);

            return Ok(result);
        }

        [HttpGet("tweets/{tweetId}/likes/count")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTweetLikesCount(
            Guid tweetId,
            CancellationToken cancellationToken)
        {
            var count = await _likeService.GetTweetLikesCountAsync(tweetId, cancellationToken);
            return Ok(count);
        }

       
        [HttpPost("comments/{commentId}/like")]
        public async Task<IActionResult> LikeComment(
            Guid commentId,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _likeService.LikeCommentAsync(userId, commentId, cancellationToken);
            return Ok(result);
        }

      
        [HttpDelete("comments/{commentId}/like")]
        public async Task<IActionResult> UnlikeComment(
            Guid commentId,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _likeService.UnlikeCommentAsync(userId, commentId, cancellationToken);
            return Ok(result);
        }

       
        [HttpGet("comments/{commentId}/like")]
        public async Task<IActionResult> IsLikedComment(
            Guid commentId,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var isLiked = await _likeService.IsLikedCommentAsync(userId, commentId, cancellationToken);
            return Ok(new { isLiked });
        }

      
        [HttpGet("comments/{commentId}/likes")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentLikes(
            Guid commentId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var result = await _likeService.GetCommentLikesAsync(
                commentId, pageNumber, pageSize, cancellationToken);

            return Ok(result);
        }

        
        [HttpGet("comments/{commentId}/likes/count")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentLikesCount(
            Guid commentId,
            CancellationToken cancellationToken)
        {
            var count = await _likeService.GetCommentLikesCountAsync(commentId, cancellationToken);
            return Ok(count);
        }
    }
}
