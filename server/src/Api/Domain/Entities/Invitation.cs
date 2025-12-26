using System;
using System.Text.Json;
using Api.Domain.ValueObjects;

namespace Api.Domain.Entities
{
    public class Invitation : Notification
    {
        public Invitation()
        {
            Token = string.Empty;
        }
        
        public Guid SentById { get; set; }
        public Guid RegardingId { get; set; }
        public string Token { get; set; }
        public string? DataJson { get; set; }
        public DateTime ExpiresAt { get; set; }
        
        public User? SentBy { get; set; }
        public Project? Regarding { get; set; }
        
        public InvitationData? Data
        {
            get => JsonSerializer.Deserialize<InvitationData>(DataJson ?? string.Empty);
            set => DataJson = JsonSerializer.Serialize(value);
        }
    }
}