using System.Threading.Tasks;
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

            if (policy == null)
            {
                return new AuthorizationPolicyBuilder()
                    .AddRequirements(new HasPermissionRequirement(policyName))
                    .Build();
            }

            return policy;
        }
    }
}