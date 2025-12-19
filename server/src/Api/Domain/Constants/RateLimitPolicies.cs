namespace Api.Domain.Constants
{
    public static class RateLimitPolicies
    {
        public const string UserTokenBucket = "user_token_bucket";
        public const string IpAddressFixedWindow = "ip_fixed_window";
    }
}