using System;

namespace Api.Infrastructure.Cache
{
    public static class PermissionCacheKeys
    {
        private const string PermissionUser = "auth:permission:user:{0}:perm:{1}";
        private const string PermissionUserProject = "auth:permission:user:{0}:project:{1}:perm:{2}";
        private const string PolicyUserProject = "auth:policy:user:{0}:project:{1}:perm:{2}";

        public static string PermissionForUser(Guid userId, string permission)
        {
            return string.Format(PermissionUser, userId, permission);
        }

        public static string PermissionForUserInProject(Guid userId, Guid projectId, string permission)
        {
            return string.Format(PermissionUserProject, userId, projectId, permission);
        }

        public static string PolicyForUserInProject(Guid userId, Guid projectId, string permission)
        {
            return string.Format(PolicyUserProject, userId, projectId, permission);
        }
    }
}