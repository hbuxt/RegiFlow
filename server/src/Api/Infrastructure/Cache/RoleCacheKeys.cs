using Api.Domain.Enums;

namespace Api.Infrastructure.Cache
{
    public static class RoleCacheKeys
    {
        private const string NameAndScopePrefix = "role:name:{0}:AND:scope:{1}";

        public static string GetByNameAndScope(string name, RoleScope scope)
        {
            return string.Format(NameAndScopePrefix, name, scope);
        }
    }
}