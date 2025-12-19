using Microsoft.AspNetCore.Authorization;

namespace Api.Infrastructure.Identity
{
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permission) : base(permission)
        {
        }
    }
}