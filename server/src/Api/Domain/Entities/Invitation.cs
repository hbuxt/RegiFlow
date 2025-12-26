using System;

namespace Api.Domain.Entities
{
    public class Invitation : Notification
    {
        public Invitation()
        {
            Token = string.Empty;
        }
        
        public Guid SentById { get; set; }
        public Guid RelatesToId { get; set; }
        public string Token { get; set; }
        public DateTime? ExpiresAt { get; set; }
        
        public User? SentBy { get; set; }
        public Project? RelatesTo { get; set; }
    }
}