using System;

namespace Api.Infrastructure.Cache
{
    public static class ProjectCacheKeys
    {
        private const string MyProjectsPrefix = "projects:createdby:id:{0}";
        private const string ProjectsImInvolvedWithPrefix = "projects:projectusers:user:id:{0}";
        private const string IdPrefix = "project:id:{0}";

        public static string GetMyProjects(Guid id) => string.Format(MyProjectsPrefix, id);
        public static string GetProjectsImInvolvedWith(Guid id) => string.Format(ProjectsImInvolvedWithPrefix, id);
        public static string GetById(Guid id) => string.Format(IdPrefix, id);
    }
}