using System;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Users.Me.Details
{
    public sealed record Response
    {
        public Response()
        {
        }

        [JsonPropertyName(FieldNames.UserId)]
        [JsonPropertyOrder(0)]
        public Guid Id { get; set; }

        [JsonPropertyName(FieldNames.FirstName)]
        [JsonPropertyOrder(1)]
        public string? FirstName { get; set; }

        [JsonPropertyName(FieldNames.LastName)]
        [JsonPropertyOrder(2)]
        public string? LastName { get; set; }

        [JsonPropertyName(FieldNames.Email)]
        [JsonPropertyOrder(3)]
        public string? Email { get; set; }
    }
}