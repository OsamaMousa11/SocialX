using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SocialX.Core.DTO.FollowDto;
using SocialX.Core.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FollowController : ControllerBase
    {
        private readonly IFollowService _followService;
        private readonly ILogger<FollowController> _logger;

        public FollowController(IFollowService followService, ILogger<FollowController> logger)
        {
            _followService = followService ?? throw new ArgumentNullException(nameof(followService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

    
        [HttpPost("follow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FollowUser([FromBody] CreateFollowDto followDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

               
                var currentUserId = GetCurrentUserId();

                var result = await _followService.FollowUserAsync(currentUserId, followDto.FollowingId, cancellationToken);

                if (!result)
                    return BadRequest(new { message = "You are already following this user or invalid follow request" });

                return Ok(new { message = "Successfully followed user", data = new { followingId = followDto.FollowingId } });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning($"Follow operation failed: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Validation error: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Follow operation was cancelled");
                return StatusCode(StatusCodes.Status499ClientClosedRequest, new { message = "Request was cancelled" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error following user: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while following user" });
            }
        }

   
        [HttpDelete("unfollow/{followingId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UnfollowUser(Guid followingId, CancellationToken cancellationToken)
        {
            try
            {
                if (followingId == Guid.Empty)
                    return BadRequest(new { message = "Invalid following ID" });

                var currentUserId = GetCurrentUserId();

                var result = await _followService.UnfollowUserAsync(currentUserId, followingId, cancellationToken);

                if (!result)
                    return BadRequest(new { message = "You are not following this user" });

                return Ok(new { message = "Successfully unfollowed user", data = new { followingId } });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Validation error: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Unfollow operation was cancelled");
                return StatusCode(StatusCodes.Status499ClientClosedRequest, new { message = "Request was cancelled" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error unfollowing user: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while unfollowing user" });
            }
        }

        [HttpGet("is-following/{followingId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> IsFollowing(Guid followingId, CancellationToken cancellationToken)
        {
            try
            {
                if (followingId == Guid.Empty)
                    return BadRequest(new { message = "Invalid following ID" });

                var currentUserId = GetCurrentUserId();
                var isFollowing = await _followService.IsFollowingAsync(currentUserId, followingId, cancellationToken);

                return Ok(new { isFollowing });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Validation error: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Follow check operation was cancelled");
                return StatusCode(StatusCodes.Status499ClientClosedRequest, new { message = "Request was cancelled" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking follow status: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while checking follow status" });
            }
        }

     
        [HttpGet("followers/{userId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFollowers(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                if (userId == Guid.Empty)
                    return BadRequest(new { message = "Invalid user ID" });

                var followers = await _followService.GetFollowersAsync(userId, cancellationToken);
                return Ok(new { count = followers.Count(), data = followers });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Validation error: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Get followers operation was cancelled");
                return StatusCode(StatusCodes.Status499ClientClosedRequest, new { message = "Request was cancelled" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching followers: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while fetching followers" });
            }
        }

     
        [HttpGet("following/{userId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFollowing(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                if (userId == Guid.Empty)
                    return BadRequest(new { message = "Invalid user ID" });

                var following = await _followService.GetFollowingAsync(userId, cancellationToken);
                return Ok(new { count = following.Count(), data = following });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Validation error: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Get following operation was cancelled");
                return StatusCode(StatusCodes.Status499ClientClosedRequest, new { message = "Request was cancelled" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching following: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while fetching following" });
            }
        }

 


        private Guid GetCurrentUserId()
        {
            var userIdClaim =
        User.FindFirst("uid") 
        ?? User.FindFirst("sub")
        ?? User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException("Unable to identify current user");

            return userId;
        }
    }
}