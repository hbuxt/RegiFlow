using System;

namespace Api.Domain.Entities
{
    public class ProjectUserRole : IEntity
    {
        public ProjectUserRole()
        {
        }
        
        public Guid Id { get; set; }
        public Guid ProjectUserId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime AssignedAt { get; set; }
        
        public ProjectUser? ProjectUser { get; set; }
        public Role? Role { get; set; }
    }
}