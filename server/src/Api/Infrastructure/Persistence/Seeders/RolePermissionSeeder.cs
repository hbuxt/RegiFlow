using System.Collections.Generic;
using Api.Domain.Entities;

namespace Api.Infrastructure.Persistence.Seeders
{
    internal static class RolePermissionSeeder
    {
        public static List<RolePermission> Generate()
        {
            var superAdministratorPermissions = new List<RolePermission>()
            {
                new RolePermission()
                {
                    RoleId = RoleSeeder.SuperAdministrator.Id,
                    PermissionId = PermissionSeeder.ViewMyDetails.Id,
                },
                new RolePermission()
                {
                    RoleId = RoleSeeder.SuperAdministrator.Id,
                    PermissionId = PermissionSeeder.UpdateMyDetails.Id,
                },
                new RolePermission()
                {
                    RoleId = RoleSeeder.SuperAdministrator.Id,
                    PermissionId = PermissionSeeder.DeleteMyDetails.Id
                },
                new RolePermission()
                {
                    RoleId = RoleSeeder.SuperAdministrator.Id,
                    PermissionId = PermissionSeeder.ViewMyRoles.Id
                }
            };
            
            var standardUserPermissions = new List<RolePermission>()
            {
                new RolePermission()
                {
                    RoleId = RoleSeeder.StandardUser.Id,
                    PermissionId = PermissionSeeder.ViewMyDetails.Id,
                },
                new RolePermission()
                {
                    RoleId = RoleSeeder.StandardUser.Id,
                    PermissionId = PermissionSeeder.UpdateMyDetails.Id,
                },
                new RolePermission()
                {
                    RoleId = RoleSeeder.StandardUser.Id,
                    PermissionId = PermissionSeeder.DeleteMyDetails.Id
                },
                new RolePermission()
                {
                    RoleId = RoleSeeder.StandardUser.Id,
                    PermissionId = PermissionSeeder.ViewMyRoles.Id
                }
            };

            var readOnlyUserPermissions = new List<RolePermission>()
            {
                new RolePermission()
                {
                    RoleId = RoleSeeder.ReadOnlyUser.Id,
                    PermissionId = PermissionSeeder.ViewMyDetails.Id
                },
                new RolePermission()
                {
                    RoleId = RoleSeeder.ReadOnlyUser.Id,
                    PermissionId = PermissionSeeder.ViewMyRoles.Id
                }
            };

            return [..superAdministratorPermissions, ..standardUserPermissions, ..readOnlyUserPermissions];
        }
    }
}