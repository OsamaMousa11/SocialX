using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialX.Api.Extensions;
using SocialX.Core.DTO.ProfileDto;
using SocialX.Core.ServiceContract;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SocialX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // كل الـ endpoints محمية
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }




        [HttpPost]
        public async Task<IActionResult> CreateProfile(
            [FromForm] ProfileAddRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var result = await _profileService.CreateProfileAsync(
                userId,
                request,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetMyProfile),
                new { },
                result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(
            [FromForm] ProfileUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var result = await _profileService.UpdateProfileAsync(
                userId,
                request,
                cancellationToken);

            return Ok(result);
        }

    
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile(
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var result = await _profileService.GetMyProfileAsync(
                userId,
                cancellationToken);

            return Ok(result);
        }

   
        [AllowAnonymous]
        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetProfileByUserId(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var result = await _profileService.GetProfileByUserIdAsync(
                userId,
                cancellationToken);

            return Ok(result);
        }

     
        [HttpDelete]
        public async Task<IActionResult> DeleteMyProfile(
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            await _profileService.DeleteMyProfileAsync(userId);

            return NoContent();
        }
    }
}
