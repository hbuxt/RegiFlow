using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Users.UpdateMyDetails
{
    public sealed record Request
    {
        public Request()
        {
        }

        [JsonPropertyName(FieldNames.FirstName)]
        public string? FirstName { get; set; }

        [JsonPropertyName(FieldNames.LastName)]
        public string? LastName { get; set; }
    }
}