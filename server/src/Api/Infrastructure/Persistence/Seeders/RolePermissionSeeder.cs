using System.Collections.Generic;
using Api.Domain.Entities;

namespace Api.Infrastructure.Persistence.Seeders
{
    internal static class RolePermissionSeeder
    {
        public static List<RolePermission> Generate()
        {
            var generalPermissions = new List<RolePermission>()
            {
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.UserRead.Id,
                    RoleId = RoleSeeder.General.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.UserUpdate.Id,
                    RoleId = RoleSeeder.General.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.UserDelete.Id,
                    RoleId = RoleSeeder.General.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.UserRolesRead.Id,
                    RoleId = RoleSeeder.General.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.UserPermissionsRead.Id,
                    RoleId = RoleSeeder.General.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectCreate.Id,
                    RoleId = RoleSeeder.General.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectRead.Id,
                    RoleId = RoleSeeder.General.Id
                }
            };

            var DemoPermissions = new List<RolePermission>()
            {
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.UserRead.Id,
                    RoleId = RoleSeeder.Demo.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.UserRolesRead.Id,
                    RoleId = RoleSeeder.Demo.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.UserPermissionsRead.Id,
                    RoleId = RoleSeeder.Demo.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectRead.Id,
                    RoleId = RoleSeeder.Demo.Id
                }
            };

            var ownerPermissions = new List<RolePermission>()
            {
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectRead.Id,
                    RoleId = RoleSeeder.Owner.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectUpdate.Id,
                    RoleId = RoleSeeder.Owner.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectDelete.Id,
                    RoleId = RoleSeeder.Owner.Id
                }
            };
            
            return [
                ..generalPermissions,
                ..DemoPermissions,
                ..ownerPermissions
            ];
        }
    }
}