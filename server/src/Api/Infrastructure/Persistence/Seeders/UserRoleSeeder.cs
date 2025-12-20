using System.Collections.Generic;
using Api.Domain.Entities;

namespace Api.Infrastructure.Persistence.Seeders
{
    internal static class UserRoleSeeder
    {
        public static List<UserRole> Generate()
        {
            var myRoles = new List<UserRole>()
            {
                new UserRole()
                {
                    UserId = UserSeeder.Me.Id,
                    RoleId = RoleSeeder.General.Id
                }
            };

            return [ ..myRoles ];
        }
    }
}