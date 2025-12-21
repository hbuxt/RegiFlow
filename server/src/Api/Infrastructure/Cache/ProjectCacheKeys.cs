using System;

namespace Api.Infrastructure.Cache
{
    public static class ProjectCacheKeys
    {
        private const string MyProjectsPrefix = "projects:createdby:id:{0}";

        public static string GetMyProjects(Guid id) => string.Format(MyProjectsPrefix, id);
    }
}