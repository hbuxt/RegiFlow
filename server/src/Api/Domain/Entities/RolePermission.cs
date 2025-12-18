using System;

namespace Api.Domain.Entities
{
    public class RolePermission : IBaseEntity
    {
        public RolePermission()
        {
        }

        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }

        public Role? Role { get; set; }
        public Permission? Permission { get; set; }
    }
}