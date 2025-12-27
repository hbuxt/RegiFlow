using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Users.RespondToMyInvitation
{
    public sealed record Request
    {
        public Request()
        {
        }
        
        [JsonPropertyName(FieldNames.InvitationStatus)]
        public string? Status { get; init; }
    }
}