using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Projects.Update
{
    public sealed record Request
    {
        public Request()
        {
        }
        
        [JsonPropertyName(FieldNames.ProjectDescription)]
        public string? Description { get; init; }
    }
}