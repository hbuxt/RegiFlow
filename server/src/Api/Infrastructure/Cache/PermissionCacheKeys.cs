using System;

namespace Api.Infrastructure.Cache
{
    public static class PermissionCacheKeys
    {
        private const string NameAndUserIdPrefix = "permission:name:{0}:AND:user:id:{1}";
        
        public static string GetByNameAndUserId(string name, Guid id) => string.Format(NameAndUserIdPrefix, name, id);
    }
}