using System;
using System.Collections.Generic;
using Api.Domain.Constants;
using Api.Domain.Entities;
using Api.Domain.Enums;

namespace Api.Infrastructure.Persistence.Seeders
{
    internal static class RoleSeeder
    {
        public static Role General { get; set; } = new Role()
        {
            Id = new Guid("EC9607B4-EEB3-4FA2-BB21-0A728CED03F1"),
            Name = RoleNames.General,
            Description = "The role assigned when registering for an account.",
            Scope = RoleScope.Application,
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static Role Demo { get; set; } = new Role()
        {
            Id = new Guid("DA91B68A-E3BF-4F88-8A72-382A9B868759"),
            Name = RoleNames.Demo,
            Description = "A role assigned for demo accounts.",
            Scope = RoleScope.Application,
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static List<Role> Generate()
        {
            return [
                General, 
                Demo
            ];
        }
    }
}