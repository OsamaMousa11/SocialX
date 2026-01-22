using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialX.Api.Extensions;
using SocialX.Core.DTO.ProfileDto;
using SocialX.Core.Service;
using SocialX.Core.ServiceContract;

namespace SocialX.API.Controllers
{
    [ApiController]
    [Route("api/profiles")]
    [Authorize]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfilesController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        // GET api/profiles/me
        [HttpGet("me")]
        public async Task<ActionResult<ProfileResponse>> GetMyProfile(
            CancellationToken ct)
        {
            var userId = User.GetUserId();
            var profile = await _profileService.GetMyProfileAsync(userId, ct);
            return Ok(profile);
        }

        // GET api/profiles/{userId}
        [HttpGet("{userId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProfileResponse>> GetProfileByUserId(
            Guid userId,
            CancellationToken ct)
        {
            var profile = await _profileService.GetProfileByUserIdAsync(userId, ct);
            return Ok(profile);
        }

 
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProfileResponse>> CreateProfile(
            [FromForm] ProfileAddRequest request,
            CancellationToken ct)
        {
            var userId = User.GetUserId();
            var profile = await _profileService.CreateProfileAsync(userId, request, ct);
            return CreatedAtAction(nameof(GetProfileByUserId), new { userId = profile.UserId }, profile);
        }

      
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProfileResponse>> UpdateProfile(
            [FromForm] ProfileUpdateRequest request,
            CancellationToken ct)
        {
            var userId = User.GetUserId();
            var profile = await _profileService.UpdateProfileAsync(userId, request, ct);
            return Ok(profile);
        }
    }
}
