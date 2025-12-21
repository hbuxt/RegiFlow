using System;

namespace Api.Infrastructure.Cache
{
    public static class PermissionCacheKeys
    {
        private const string NameAndUserIdPrefix = "permission:name:{0}:AND:user:id:{1}";
        private const string NameAndUserIdInProjectPrefix = "permission:name:{0}:AND:user:id:{1}:AND:project:{2}";
        
        public static string GetByNameAndUserId(string name, Guid id) => string.Format(NameAndUserIdPrefix, name, id);
        public static string GetByNameAndUserIdInProject(string name, Guid userId, Guid projectId) => string.Format(NameAndUserIdInProjectPrefix, name, userId, projectId);
    }
}