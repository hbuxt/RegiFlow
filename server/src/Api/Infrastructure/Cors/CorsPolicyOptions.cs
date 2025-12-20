using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Infrastructure.Cors
{
    public sealed class CorsPolicyOptions
    {
        public CorsPolicyOptions()
        {
            Origins = [];
            Headers = [];
        }
        
        [Required]
        [JsonPropertyName("Origins")]
        public string[] Origins { get; set; }
        
        [Required]
        [JsonPropertyName("Headers")]
        public string[] Headers { get; set; }
    }
}