using System;
using System.Security.Claims;

namespace SocialX.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("UserId claim not found");

            return Guid.Parse(userIdClaim.Value);
        }
    }
}
