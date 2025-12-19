namespace Api.Infrastructure.Cache
{
    public interface ICacheOptions
    {
        bool IsEnabled { get; }
        int LengthInMinutes { get; }
    }
}