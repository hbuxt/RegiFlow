using System;

namespace Api.Infrastructure.Cache
{
    public static class UserCacheKeys
    {
        private const string ById = "users:id:{0}";
        private const string ByEmail = "users:email:{0}";
        private const string RolesById = "users:id:{0}:roles";

        public static string ForId(Guid userId) => string.Format(ById, userId);
        public static string ForEmail(string email) => string.Format(ByEmail, email);
        public static string RolesForId(Guid userId) => string.Format(RolesById, userId);
    }
}