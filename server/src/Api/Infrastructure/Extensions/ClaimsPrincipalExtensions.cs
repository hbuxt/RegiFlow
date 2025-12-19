using System;
using System.Security.Claims;

namespace Api.Infrastructure.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var userClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim == null || !Guid.TryParse(userClaim.Value, out var id))
            {
                return null;
            }

            return id;
        }
    }
}