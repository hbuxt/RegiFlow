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
                }
            };
        }
    }
}