using Api.Domain.Enums;

namespace Api.Infrastructure.Cache
{
    public static class RoleCacheKeys
    {
        private const string ByNameAndScope = "roles:name:{0}:scope:{1}";

        public static string ForNameAndScope(string roleName, RoleScope scope)
        {
            return string.Format(ByNameAndScope, roleName, scope);
        }
    }
}