using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialX.Core.ServiceContract;
using SocialX.Api.Extensions;

namespace SocialX.Api.Controllers
{
    [ApiController]
    [Route("api/me/mentions")]
    [Authorize]
    public class MentionController : ControllerBase
    {
        private readonly IMentionService _mentionService;

        public MentionController(IMentionService mentionService)
        {
            _mentionService = mentionService;
        }

      
        [HttpGet]
        public async Task<IActionResult> GetMyMentions(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var userId = User.GetUserId();

            var result = await _mentionService.GetUserMentionsAsync(
                userId,
                pageNumber,
                pageSize,
                cancellationToken);

            return Ok(result);
        }
    }
}
