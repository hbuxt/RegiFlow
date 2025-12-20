using System.Collections.Generic;
using Api.Domain.Entities;

namespace Api.Infrastructure.Persistence.Seeders
{
    internal static class UserRoleSeeder
    {
        public static List<UserRole> Generate()
        {
            return new List<UserRole>()
            {
                new UserRole()
                {
                    UserId = UserSeeder.DefaultAdministrator.Id,
                    RoleId = RoleSeeder.SuperAdministrator.Id
                },
                new UserRole()
                {
                    UserId = UserSeeder.DefaultAdministrator.Id,
                    RoleId = RoleSeeder.StandardUser.Id
                },
                new UserRole()
                {
                    UserId = UserSeeder.DemoUser1.Id,
                    RoleId = RoleSeeder.ReadOnlyUser.Id
                },
                new UserRole()
                {
                    UserId = UserSeeder.DemoUser2.Id,
                    RoleId = RoleSeeder.ReadOnlyUser.Id
                },
                new UserRole()
                {
                    UserId = UserSeeder.DemoUser3.Id,
                    RoleId = RoleSeeder.ReadOnlyUser.Id
                },
                new UserRole()
                {
                    UserId = UserSeeder.DemoUser4.Id,
                    RoleId = RoleSeeder.ReadOnlyUser.Id
                },
                new UserRole()
                {
                    UserId = UserSeeder.DemoUser5.Id,
                    RoleId = RoleSeeder.ReadOnlyUser.Id
                }
            };
        }
    }
}