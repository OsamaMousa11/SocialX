using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialX.Api.Extensions;
using SocialX.Core.ServiceContract;

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

    [HttpPost("tweets/{tweetId}/like")]
    public async Task<IActionResult> LikeTweet(Guid tweetId, CancellationToken ct)
    {
        var userId = User.GetUserId();
        await _likeService.LikeTweetAsync(userId, tweetId, ct);
        return Ok();
    }

    [HttpDelete("tweets/{tweetId}/like")]
    public async Task<IActionResult> UnlikeTweet(Guid tweetId, CancellationToken ct)
    {
        var userId = User.GetUserId();
        await _likeService.UnlikeTweetAsync(userId, tweetId, ct);
        return Ok();
    }

    [HttpGet("tweets/{tweetId}/like")]
    public async Task<IActionResult> IsLikedTweet(Guid tweetId, CancellationToken ct)
    {
        var userId = User.GetUserId();
        return Ok(await _likeService.IsLikedTweetAsync(userId, tweetId, ct));
    }




    [HttpPost("comments/{commentId}/like")]
    public async Task<IActionResult> LikeComment(Guid commentId, CancellationToken ct)
    {
        var userId = User.GetUserId();
        await _likeService.LikeCommentAsync(userId, commentId, ct);
        return Ok();
    }

    [HttpDelete("comments/{commentId}/like")]
    public async Task<IActionResult> UnlikeComment(Guid commentId, CancellationToken ct)
    {
        var userId = User.GetUserId();
        await _likeService.UnlikeCommentAsync(userId, commentId, ct);
        return Ok();
    }

    [HttpGet("comments/{commentId}/like")]
    public async Task<IActionResult> IsLikedComment(Guid commentId, CancellationToken ct)
    {
        var userId = User.GetUserId();
        return Ok(await _likeService.IsLikedCommentAsync(userId, commentId, ct));
    }


}
