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

        public static Role Owner { get; set; } = new Role()
        {
            Id = new Guid("B4D50721-7C41-491B-92D7-A8213599CC2B"),
            Name = RoleNames.Owner,
            Description = "Allows the user to perform all project operations, including project deletion.",
            Scope = RoleScope.Project,
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Role Admin { get; set; } = new Role()
        {
            Id = new Guid("1FBB98C3-37AC-4DEF-8824-83032CFDFB54"),
            Name = RoleNames.Admin,
            Description = "Allows the user to read and update tickets, invite and kick users, and update user roles",
            Scope = RoleScope.Project,
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Role Developer { get; set; } = new Role()
        {
            Id = new Guid("625484D1-E87E-4DF0-9997-B53BBD150DF0"),
            Name = RoleNames.Developer,
            Description = "Allows the user to read and update tickets",
            Scope = RoleScope.Project,
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Role Viewer { get; set; } = new Role()
        {
            Id = new Guid("FE40C9DD-0205-4231-A9E7-DA887545636A"),
            Name = RoleNames.Viewer,
            Description = "Restricts the user to reading data.",
            Scope = RoleScope.Project,
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static List<Role> Generate()
        {
            return [
                General, 
                Demo,
                Owner,
                Admin,
                Developer,
                Viewer
            ];
        }
    }
}