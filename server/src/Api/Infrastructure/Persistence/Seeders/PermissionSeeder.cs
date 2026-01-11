using System;
using System.Collections.Generic;
using Api.Domain.Constants;
using Api.Domain.Entities;

namespace Api.Infrastructure.Persistence.Seeders
{
    internal static class PermissionSeeder
    {
        public static Permission UserRead { get; set; } = new Permission()
        {
            Id = new Guid("6B8A28D1-D8EB-46D4-9946-B6007DBB7C23"),
            Name = PermissionNames.UserRead,
            Description = "Allows the user to view their details.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static Permission UserProfileUpdate { get; set; } = new Permission()
        {
            Id = new Guid("37FAD974-167B-4D6F-9CC6-C57B488B72A7"),
            Name = PermissionNames.UserProfileUpdate,
            Description = "Allows the user to update their details.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static Permission UserDelete { get; set; } = new Permission()
        {
            Id = new Guid("3FC973AF-C16F-4A92-A461-3CCE8F5CECF9"),
            Name = PermissionNames.UserDelete,
            Description = "Allows the user to delete their details.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Permission UserInvitationsUpdate { get; set; } = new Permission()
        {
            Id = new Guid("87EDFA7A-1FCD-4114-BEA8-4B89CD21FBDE"),
            Name = PermissionNames.UserInvitationsUpdate,
            Description = "Allows the user to respond to invitations.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static Permission UserRolesRead { get; set; } = new Permission()
        {
            Id = new Guid("0E9B858F-164D-4C75-A559-9C13D3794547"),
            Name = PermissionNames.UserRolesRead,
            Description = "Allows the user to view their roles.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static Permission UserPermissionsRead { get; set; } = new Permission()
        {
            Id = new Guid("3F396475-3E5A-4C44-93C0-77ACC30E494F"),
            Name = PermissionNames.UserPermissionsRead,
            Description = "Allows the user to view their permissions.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Permission UserNotificationsRead { get; set; } = new Permission()
        {
            Id = new Guid("63727AF0-737F-4F10-9320-1FA0FFDFD540"),
            Name = PermissionNames.UserNotificationsRead,
            Description = "Allows the user to view their notifications.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Permission RolesRead { get; set; } = new Permission()
        {
            Id = new Guid("6A856BB1-3865-4BB2-9FA9-8CC74968F1E0"),
            Name = PermissionNames.RolesRead,
            Description = "Allows the user to view system roles.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Permission ProjectRead { get; set; } = new Permission()
        {
            Id = new Guid("7B91219A-11FF-46C3-88B3-BD483C3A1658"),
            Name = PermissionNames.ProjectRead,
            Description = "Allows the user to view project they're involved with.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static Permission ProjectCreate { get; set; } = new Permission()
        {
            Id = new Guid("73FBC56A-BE18-45D8-BB78-5FD8B0391B6C"),
            Name = PermissionNames.ProjectCreate,
            Description = "Allows the user to create projects.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Permission ProjectNameUpdate { get; set; } = new Permission()
        {
            Id = new Guid("D132729B-1009-48D2-A6C1-17761C8FF500"),
            Name = PermissionNames.ProjectNameUpdate,
            Description = "Allows the user to rename a project.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Permission ProjectDescriptionUpdate { get; set; } = new Permission()
        {
            Id = new Guid("793454CA-E380-44A4-AD3F-BD3BCFB53DC3"),
            Name = PermissionNames.ProjectDescriptionUpdate,
            Description = "Allows the user to update a project description.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Permission ProjectDelete { get; set; } = new Permission()
        {
            Id = new Guid("2AD51633-09B5-4ABC-8CD7-0FEF16CA08DE"),
            Name = PermissionNames.ProjectDelete,
            Description = "Allows the user to delete a project.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Permission ProjectUsersRead { get; set; } = new Permission()
        {
            Id = new Guid("9385F62B-2867-42C6-9DC0-3598D19DA8F8"),
            Name = PermissionNames.ProjectUsersRead,
            Description = "Allows the user to view the users in a project.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Permission ProjectUsersUpdate { get; set; } = new Permission()
        {
            Id = new Guid("FFBC21EF-EBAB-4459-ADD6-7AC5D748C416"),
            Name = PermissionNames.ProjectUsersUpdate,
            Description = "Allows the user to change the user's roles in a project.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static Permission ProjectPermissionsRead { get; set; } = new Permission()
        {
            Id = new Guid("5967EAC1-DABF-4C13-880A-3B25C4078A4F"),
            Name = PermissionNames.ProjectPermissionsRead,
            Description = "Allows the user to view their permissions in a project.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Permission ProjectInvitationInvite { get; set; } = new Permission()
        {
            Id = new Guid("8C2A9606-A6FA-4D1B-8068-1F3A4767EDA2"),
            Name = PermissionNames.ProjectInvitationsCreate,
            Description = "Allows the user to invite users into a project.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static Permission ProjectInvitationRevoke { get; set; } = new Permission()
        {
            Id = new Guid("3AE302BE-A68C-4A1F-827C-B35EDABDF0BB"),
            Name = PermissionNames.ProjectInvitationsDelete,
            Description = "Allows the user to revoke invitations in a project.",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static List<Permission> Generate()
        {
            return [
                UserRead,
                UserProfileUpdate,
                UserDelete,
                UserInvitationsUpdate,
                UserRolesRead,
                UserPermissionsRead,
                UserNotificationsRead,
                RolesRead,
                ProjectCreate,
                ProjectRead,
                ProjectNameUpdate,
                ProjectDescriptionUpdate,
                ProjectDelete,
                ProjectUsersRead,
                ProjectUsersUpdate,
                ProjectPermissionsRead,
                ProjectInvitationInvite,
                ProjectInvitationRevoke
            ];
        }
    }
}