using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Users.UpdateMyProfile
{
    public sealed record Request
    {
        public Request()
        {
        }

        [JsonPropertyName(FieldNames.FirstName)]
        public string? FirstName { get; init; }

        [JsonPropertyName(FieldNames.LastName)]
        public string? LastName { get; init; }
    }
}