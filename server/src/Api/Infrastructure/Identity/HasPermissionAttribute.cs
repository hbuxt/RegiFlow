using Api.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Api.Infrastructure.Identity
{
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permission, PermissionScope scope = PermissionScope.Global)
        {
            Policy = $"{permission}:{scope}";
        }
    }
}