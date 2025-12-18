using System;
using System.Collections.Generic;

namespace Api.Domain.Entities
{
    public class User : IEntity
    {
        public User()
        {
            Email = string.Empty;
            HashedPassword = string.Empty;
            UserRoles = new List<UserRole>();
        }
        
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        
        public ICollection<UserRole> UserRoles { get; set; }
    }
}