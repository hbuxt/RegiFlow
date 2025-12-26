using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Auth.Login
{
    public sealed record Response
    {
        public Response()
        {
        }

        [JsonPropertyName(FieldNames.AccessToken)]
        [JsonPropertyOrder(0)]
        public string? AccessToken { get; init; }
    }
}