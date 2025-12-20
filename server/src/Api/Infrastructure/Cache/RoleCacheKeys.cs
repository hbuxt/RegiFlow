namespace Api.Infrastructure.Cache
{
    public static class RoleCacheKeys
    {
        private const string NamePrefix = "role:name:{0}";
        
        public static string GetByName(string name) => string.Format(NamePrefix, name);
    }
}