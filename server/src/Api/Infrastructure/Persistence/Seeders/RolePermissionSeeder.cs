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
                    PermissionId = PermissionSeeder.ViewMyDetails.Id,
                    RoleId = RoleSeeder.General.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.UpdateMyDetails.Id,
                    RoleId = RoleSeeder.General.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.DeleteMyDetails.Id,
                    RoleId = RoleSeeder.General.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ViewMyRoles.Id,
                    RoleId = RoleSeeder.General.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.CreateProjects.Id,
                    RoleId = RoleSeeder.General.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ViewMyProjects.Id,
                    RoleId = RoleSeeder.General.Id
                }
            };

            var DemoPermissions = new List<RolePermission>()
            {
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ViewMyDetails.Id,
                    RoleId = RoleSeeder.Demo.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ViewMyRoles.Id,
                    RoleId = RoleSeeder.Demo.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ViewMyProjects.Id,
                    RoleId = RoleSeeder.Demo.Id
                }
            };

            var ownerPermissions = new List<RolePermission>()
            {
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ViewProject.Id,
                    RoleId = RoleSeeder.Owner.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.RenameProject.Id,
                    RoleId = RoleSeeder.Owner.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.UpdateProject.Id,
                    RoleId = RoleSeeder.Owner.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.DeleteProject.Id,
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