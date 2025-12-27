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
                    PermissionId = PermissionSeeder.UserInvitationsUpdate.Id,
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
                    PermissionId = PermissionSeeder.UserNotificationsRead.Id,
                    RoleId = RoleSeeder.General.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.RolesRead.Id,
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
                    PermissionId = PermissionSeeder.UserNotificationsRead.Id,
                    RoleId = RoleSeeder.Demo.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.RolesRead.Id,
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
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectUsersRead.Id,
                    RoleId = RoleSeeder.Owner.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectUsersUpdate.Id,
                    RoleId = RoleSeeder.Owner.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectPermissionsRead.Id,
                    RoleId = RoleSeeder.Owner.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectInvitationInvite.Id,
                    RoleId = RoleSeeder.Owner.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectInvitationRevoke.Id,
                    RoleId = RoleSeeder.Owner.Id
                }
            };

            var adminPermissions = new List<RolePermission>()
            {
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectRead.Id,
                    RoleId = RoleSeeder.Admin.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectUpdate.Id,
                    RoleId = RoleSeeder.Admin.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectUsersRead.Id,
                    RoleId = RoleSeeder.Admin.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectUsersUpdate.Id,
                    RoleId = RoleSeeder.Admin.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectPermissionsRead.Id,
                    RoleId = RoleSeeder.Admin.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectInvitationInvite.Id,
                    RoleId = RoleSeeder.Admin.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectInvitationRevoke.Id,
                    RoleId = RoleSeeder.Admin.Id
                }
            };

            var developerPermissions = new List<RolePermission>()
            {
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectRead.Id,
                    RoleId = RoleSeeder.Developer.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectUsersRead.Id,
                    RoleId = RoleSeeder.Developer.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectPermissionsRead.Id,
                    RoleId = RoleSeeder.Developer.Id
                },
            };

            var viewerPermissions = new List<RolePermission>()
            {
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectRead.Id,
                    RoleId = RoleSeeder.Viewer.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectUsersRead.Id,
                    RoleId = RoleSeeder.Viewer.Id
                },
                new RolePermission()
                {
                    PermissionId = PermissionSeeder.ProjectPermissionsRead.Id,
                    RoleId = RoleSeeder.Viewer.Id
                },
            };
            
            return [
                ..generalPermissions,
                ..DemoPermissions,
                ..ownerPermissions,
                ..adminPermissions,
                ..developerPermissions,
                ..viewerPermissions
            ];
        }
    }
}