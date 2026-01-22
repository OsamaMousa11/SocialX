using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialX.Core.DTO.Common;
using SocialX.Core.DTO.HashtagDto;
using SocialX.Core.ServiceContract;

namespace SocialX.API.Controllers
{
    [ApiController]
    [Route("api/hashtags")]
    public class HashtagsController : ControllerBase
    {
        private readonly IHashtagService _hashtagService;

        public HashtagsController(IHashtagService hashtagService)
        {
            _hashtagService = hashtagService;
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            [FromBody] CreateHashtagRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _hashtagService.CreateHashtagAsync(
                request.Name,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { hashtagId = result.Id },
                result);
        }

       

        [HttpGet("{hashtagId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(
            Guid hashtagId,
            CancellationToken cancellationToken)
        {
            var result = await _hashtagService.GetHashtagByIdAsync(
                hashtagId,
                cancellationToken);

            return Ok(result);
        }

     
        [HttpGet("name/{tagName}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByName(
            string tagName,
            CancellationToken cancellationToken)
        {
            var result = await _hashtagService.GetHashtagByNameAsync(
                tagName,
                cancellationToken);

            return Ok(result);
        }

 
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search(
            [FromQuery] string term,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await _hashtagService.SearchHashtagsAsync(
                term,
                pageNumber,
                pageSize,
                cancellationToken);

            return Ok(result);
        }


        [HttpDelete("{hashtagId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(
            Guid hashtagId,
            CancellationToken cancellationToken)
        {
            await _hashtagService.DeleteHashtagAsync(
                hashtagId,
                cancellationToken);

            return Ok(new { message = "Hashtag deleted successfully" });
        }
    }
}
