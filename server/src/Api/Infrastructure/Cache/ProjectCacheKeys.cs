using System;

namespace Api.Infrastructure.Cache
{
    public static class ProjectCacheKeys
    {
        private const string ByCreator = "projects:creator:{0}";
        private const string ByUser = "projects:user:{0}";
        private const string ById = "projects:id:{0}";

        public static string ForCreator(Guid creatorId) => string.Format(ByCreator, creatorId);
        public static string ForUser(Guid userId) => string.Format(ByUser, userId);
        public static string ForId(Guid projectId) => string.Format(ById, projectId);
    }
}