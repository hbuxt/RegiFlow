using System;
using System.Collections.Generic;
using Api.Domain.Enums;

namespace Api.Domain.Entities
{
    public class Role : IEntity
    {
        public Role()
        {
            Name = string.Empty;
            UserRoles = new List<UserRole>();
            RolePermissions = new List<RolePermission>();
            ProjectUserRoles = new List<ProjectUserRole>();
        }
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public RoleScope Scope { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
        public ICollection<ProjectUserRole> ProjectUserRoles { get; set; }
    }
}