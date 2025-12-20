using System;
using System.Collections.Generic;

namespace Api.Domain.Entities
{
    public class ProjectUser : IEntity
    {
        public ProjectUser()
        {
            ProjectUserRoles = new List<ProjectUserRole>();
        }
        
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public DateTime JoinedAt { get; set; }
        
        public Project? Project { get; set; }
        public User? User { get; set; }
        public ICollection<ProjectUserRole> ProjectUserRoles { get; set; }
    }
}