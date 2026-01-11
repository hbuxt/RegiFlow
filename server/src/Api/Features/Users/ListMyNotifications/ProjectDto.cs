using System;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Users.ListMyNotifications
{
    public sealed record ProjectDto
    {
        public ProjectDto()
        {
        }
        
        [JsonPropertyName(FieldNames.ProjectId)]
        [JsonPropertyOrder(0)]
        public Guid? Id { get; init; }
        
        [JsonPropertyName(FieldNames.ProjectName)]
        [JsonPropertyOrder(0)]
        public string? Name { get; init; }
        
        [JsonPropertyName(FieldNames.ProjectDescription)]
        [JsonPropertyOrder(1)]
        public string? Description { get; init; }
    }
}