using System;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Users.ListMyProjects
{
    public sealed record ProjectDto
    {
        public ProjectDto()
        {
        }
        
        [JsonPropertyName(FieldNames.ProjectId)]
        [JsonPropertyOrder(0)]
        public Guid Id { get; init; }
        
        [JsonPropertyName(FieldNames.ProjectName)]
        [JsonPropertyOrder(0)]
        public string? Name { get; set; }
        
        [JsonPropertyName(FieldNames.ProjectCreatedAt)]
        [JsonPropertyOrder(0)]
        public DateTime CreatedAt { get; init; }
        
        [JsonPropertyName(FieldNames.ProjectCreatedBy)]
        [JsonPropertyOrder(0)]
        public UserDto? CreatedBy { get; init; }
    }
}