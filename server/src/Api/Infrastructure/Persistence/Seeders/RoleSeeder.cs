using System;
using System.Collections.Generic;
using Api.Domain.Constants;
using Api.Domain.Entities;
using Api.Domain.Enums;

namespace Api.Infrastructure.Persistence.Seeders
{
    internal static class RoleSeeder
    {
        public static Role SuperAdministrator { get; set; } = new Role()
        {
            Id = new Guid("76477F30-8761-49EB-94CE-5685AF2112D6"),
            Name = RoleNames.SuperAdministrator,
            Description = "Allows a user to perform tasks which require elevated permissions.",
            Scope = RoleScope.Application,
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static Role StandardUser { get; set; } = new Role()
        {
            Id = new Guid("EC9607B4-EEB3-4FA2-BB21-0A728CED03F1"),
            Name = RoleNames.StandardUser,
            Description = "Allows a user to perform most actions of the application.",
            Scope = RoleScope.Application,
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static Role ReadOnlyUser { get; set; } = new Role()
        {
            Id = new Guid("DA91B68A-E3BF-4F88-8A72-382A9B868759"),
            Name = RoleNames.ReadOnlyUser,
            Description = "Prevents a user from performing write actions in the application.",
            Scope = RoleScope.Application,
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static List<Role> Generate()
        {
            return [SuperAdministrator, StandardUser, ReadOnlyUser];
        }
    }
}