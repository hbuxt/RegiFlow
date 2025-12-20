using System;
using System.Collections.Generic;
using Api.Domain.Constants;
using Api.Domain.Entities;

namespace Api.Infrastructure.Persistence.Seeders
{
    internal static class PermissionSeeder
    {
        public static Permission ViewMyDetails { get; set; } = new Permission()
        {
            Id = new Guid("6B8A28D1-D8EB-46D4-9946-B6007DBB7C23"),
            Name = PermissionNames.ViewMyDetails,
            Description = "Allows the user to view their details.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static Permission UpdateMyDetails { get; set; } = new Permission()
        {
            Id = new Guid("37FAD974-167B-4D6F-9CC6-C57B488B72A7"),
            Name = PermissionNames.UpdateMyDetails,
            Description = "Allows the user to update their details.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static Permission DeleteMyDetails { get; set; } = new Permission()
        {
            Id = new Guid("3FC973AF-C16F-4A92-A461-3CCE8F5CECF9"),
            Name = PermissionNames.DeleteMyDetails,
            Description = "Allows the user to delete their details.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static Permission ViewMyRoles { get; set; } = new Permission()
        {
            Id = new Guid("0E9B858F-164D-4C75-A559-9C13D3794547"),
            Name = PermissionNames.ViewMyRoles,
            Description = "Allows the user to view their roles.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static Permission CreateProjects { get; set; } = new Permission()
        {
            Id = new Guid("73FBC56A-BE18-45D8-BB78-5FD8B0391B6C"),
            Name = PermissionNames.CreateProjects,
            Description = "Allows the user to create projects.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static List<Permission> Generate()
        {
            return [
                ViewMyDetails, 
                UpdateMyDetails, 
                DeleteMyDetails,
                ViewMyRoles,
                CreateProjects
            ];
        }
    }
}