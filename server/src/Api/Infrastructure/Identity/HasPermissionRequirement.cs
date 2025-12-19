using Microsoft.AspNetCore.Authorization;

namespace Api.Infrastructure.Identity
{
    internal sealed class HasPermissionRequirement : IAuthorizationRequirement
    {
        public HasPermissionRequirement(string permission)
        {
            Permission = permission;
        }
        
        public string Permission { get; private set; }
    }
}