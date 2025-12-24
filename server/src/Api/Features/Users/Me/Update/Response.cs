using System;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Users.Me.Update
{
    public sealed record Response
    {
        public Response()
        {
        }

        [JsonPropertyName(FieldNames.UserId)]
        public Guid Id { get; set; }
    }
}