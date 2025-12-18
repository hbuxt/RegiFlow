using System;
using System.Collections.Generic;

namespace Api.Domain.Entities
{
    public class Permission : IEntity
    {
        public Permission()
        {
            Name = string.Empty;
            RolePermissions = new List<RolePermission>();
        }
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}