using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Projects.Create
{
    public sealed record Request
    {
        public Request()
        {
        }
        
        [JsonPropertyName(FieldNames.ProjectName)]
        public string? Name { get; init; }
        
        [JsonPropertyName(FieldNames.ProjectDescription)]
        public string? Description { get; init; }
    }
}