using System;

namespace Api.Infrastructure.Cache
{
    public static class ProjectCacheKeys
    {
        private const string ByCreatorPrefix = "projects:createdby:id:{0}";
        private const string ByUserPrefix = "projects:projectusers:user:id:{0}";
        private const string IdPrefix = "project:id:{0}";

        public static string GetByCreator(Guid id) => string.Format(ByCreatorPrefix, id);
        public static string GetByUser(Guid id) => string.Format(ByUserPrefix, id);
        public static string GetById(Guid id) => string.Format(IdPrefix, id);
    }
}