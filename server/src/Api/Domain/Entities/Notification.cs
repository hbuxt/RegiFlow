using System;
using Api.Domain.Enums;

namespace Api.Domain.Entities
{
    public class Notification : IEntity
    {
        public Notification()
        {
        }
        
        public Guid Id { get; set; }
        public Guid RecipientId { get; set; }
        public NotificationType Type { get; set; }
        public NotificationStatus Status { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public User? Recipient { get; set; }
    }
}