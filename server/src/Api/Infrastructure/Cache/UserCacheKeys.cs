using System;

namespace Api.Infrastructure.Cache
{
    public static class UserCacheKeys
    {
        private const string IdPrefix = "user:id:{0}";
        private const string EmailPrefix = "user:email:{0}";
        private const string RolesByIdPrefix = "user:id:{0}:roles";
        
        public static string GetById(Guid id) => string.Format(IdPrefix, id);
        public static string GetByEmail(string email) => string.Format(EmailPrefix, email);
        public static string GetRolesById(Guid id) => string.Format(RolesByIdPrefix, id);
    }
}