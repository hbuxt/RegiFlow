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
        
        public static Permission ViewMyProjects { get; set; } = new Permission()
        {
            Id = new Guid("7B91219A-11FF-46C3-88B3-BD483C3A1658"),
            Name = PermissionNames.ViewMyProjects,
            Description = "Allows the user to view project they're involved with.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static Permission CreateProjects { get; set; } = new Permission()
        {
            Id = new Guid("73FBC56A-BE18-45D8-BB78-5FD8B0391B6C"),
            Name = PermissionNames.CreateProjects,
            Description = "Allows the user to create projects.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Permission ViewProject { get; set; } = new Permission()
        {
            Id = new Guid("AB80F393-242F-4021-9CCC-F93C5D11427E"),
            Name = PermissionNames.ViewProject,
            Description = "Allows the user to view a project.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Permission RenameProject { get; set; } = new Permission()
        {
            Id = new Guid("008824E5-561A-4C51-B9CB-3D75782F1B84"),
            Name = PermissionNames.RenameProject,
            Description = "Allows the user to rename a project.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Permission UpdateProject { get; set; } = new Permission()
        {
            Id = new Guid("D132729B-1009-48D2-A6C1-17761C8FF500"),
            Name = PermissionNames.UpdateProject,
            Description = "Allows the user to update a project.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Permission DeleteProject { get; set; } = new Permission()
        {
            Id = new Guid("2AD51633-09B5-4ABC-8CD7-0FEF16CA08DE"),
            Name = PermissionNames.DeleteProject,
            Description = "Allows the user to delete a project.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static List<Permission> Generate()
        {
            return [
                ViewMyDetails, 
                UpdateMyDetails, 
                DeleteMyDetails,
                ViewMyRoles,
                ViewMyProjects,
                CreateProjects,
                ViewProject,
                RenameProject,
                UpdateProject,
                DeleteProject
            ];
        }
    }
}