using System;

namespace Api.Domain.Entities
{
    public class UserRole : IBaseEntity
    {
        public UserRole()
        {
        }

        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public User? User { get; set; }
        public Role? Role { get; set; }
    }
}