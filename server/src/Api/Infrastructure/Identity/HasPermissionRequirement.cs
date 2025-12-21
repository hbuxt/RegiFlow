using Api.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Api.Infrastructure.Identity
{
    internal sealed class HasPermissionRequirement : IAuthorizationRequirement
    {
        public HasPermissionRequirement(string permission, PermissionScope scope = PermissionScope.Global)
        {
            Permission = permission;
            Scope = scope;
        }
        
        public string Permission { get; private set; }
        public PermissionScope Scope { get; private set; }
    }
}