using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Users.Me.Notifications
{
    public sealed record ProjectDto
    {
        public ProjectDto()
        {
        }
        
        [JsonPropertyName(FieldNames.ProjectName)]
        [JsonPropertyOrder(0)]
        public string? Name { get; init; }
    }
}