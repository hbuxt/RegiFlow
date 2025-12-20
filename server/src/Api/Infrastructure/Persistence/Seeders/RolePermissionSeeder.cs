using System.Collections.Generic;
using Api.Domain.Entities;

namespace Api.Infrastructure.Persistence.Seeders
{
    internal static class RolePermissionSeeder
    {
        public static List<RolePermission> Generate()
        {
            var applicationAdministratorPermissions = new List<RolePermission>()
            {
                new RolePermission()
                {
                    RoleId = RoleSeeder.ApplicationAdministrator.Id,
                    PermissionId = PermissionSeeder.ViewMyDetails.Id,
                },
                new RolePermission()
                {
                    RoleId = RoleSeeder.ApplicationAdministrator.Id,
                    PermissionId = PermissionSeeder.UpdateMyDetails.Id,
                },
                new RolePermission()
                {
                    RoleId = RoleSeeder.ApplicationAdministrator.Id,
                    PermissionId = PermissionSeeder.DeleteMyDetails.Id
                },
                new RolePermission()
                {
                    RoleId = RoleSeeder.ApplicationAdministrator.Id,
                    PermissionId = PermissionSeeder.ViewMyRoles.Id
                }
            };
            
            var applicationUserPermissions = new List<RolePermission>()
            {
                new RolePermission()
                {
                    RoleId = RoleSeeder.ApplicationUser.Id,
                    PermissionId = PermissionSeeder.ViewMyDetails.Id,
                },
                new RolePermission()
                {
                    RoleId = RoleSeeder.ApplicationUser.Id,
                    PermissionId = PermissionSeeder.UpdateMyDetails.Id,
                },
                new RolePermission()
                {
                    RoleId = RoleSeeder.ApplicationUser.Id,
                    PermissionId = PermissionSeeder.DeleteMyDetails.Id
                },
                new RolePermission()
                {
                    RoleId = RoleSeeder.ApplicationUser.Id,
                    PermissionId = PermissionSeeder.ViewMyRoles.Id
                }
            };

            var applicationViewerPermissions = new List<RolePermission>()
            {
                new RolePermission()
                {
                    RoleId = RoleSeeder.ApplicationViewer.Id,
                    PermissionId = PermissionSeeder.ViewMyDetails.Id
                },
                new RolePermission()
                {
                    RoleId = RoleSeeder.ApplicationViewer.Id,
                    PermissionId = PermissionSeeder.ViewMyRoles.Id
                }
            };

            return [
                ..applicationAdministratorPermissions, 
                ..applicationUserPermissions, 
                ..applicationViewerPermissions
            ];
        }
    }
}