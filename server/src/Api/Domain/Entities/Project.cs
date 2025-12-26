using System;
using System.Collections.Generic;

namespace Api.Domain.Entities
{
    public class Project : IEntity
    {
        public Project()
        {
            Name = string.Empty;
            ProjectUsers = new List<ProjectUser>();
            Invitations = new List<Invitation>();
        }
        
        public Guid Id { get; set; }
        public Guid CreatedById { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public User? CreatedBy { get; set; }
        public ICollection<ProjectUser> ProjectUsers { get; set; }
        public ICollection<Invitation> Invitations { get; set; }
    }
}