using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Infrastructure.Cache
{
    public sealed class RoleCacheOptions : ICacheOptions
    {
        public RoleCacheOptions()
        {
            IsEnabled = false;
            LengthInMinutes = 10;
        }
        
        [Required]
        [JsonPropertyName("IsEnabled")]
        public bool IsEnabled { get; set; }

        [Required]
        [Range(0, 60)]
        [JsonPropertyName("LengthInMinutes")]
        public int LengthInMinutes { get; set; }
    }
}