using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Infrastructure.Identity
{
    public sealed class JwtBearerOptions
    {
        public JwtBearerOptions()
        {
            Secret = string.Empty;
            Issuer = string.Empty;
            Audience = string.Empty;
            ExpiryInMinutes = 0;
        }
        
        [Required]
        [JsonPropertyName("Secret")]
        public string Secret { get; set; }
        
        [Required]
        [JsonPropertyName("Issuer")]
        public string Issuer { get; set; }
        
        [Required]
        [JsonPropertyName("Audience")]
        public string Audience { get; set; }
        
        [Required]
        [JsonPropertyName("ExpiryInMinutes")]
        public int ExpiryInMinutes { get; set; }
    }
}