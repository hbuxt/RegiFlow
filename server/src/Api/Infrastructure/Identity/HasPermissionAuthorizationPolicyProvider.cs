using System;
using System.Threading.Tasks;
using Api.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Api.Infrastructure.Identity
{
    internal sealed class HasPermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public HasPermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
        }
        
        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);

            if (policy != null)
            {
                return policy;
            }

            var parts = policyName.Split(':', count: 2);
                
            if (parts.Length != 2)
            {
                return null;
            }
            
            var permission = parts[0];
            _ = Enum.TryParse<PermissionScope>(parts[1], out var scope);
                
            return new AuthorizationPolicyBuilder()
                .AddRequirements(new HasPermissionRequirement(permission, scope))
                .Build();
        }
    }
}