using System;
using System.Collections.Generic;
using Api.Domain.Entities;

namespace Api.Infrastructure.Persistence.Seeders
{
    internal static class UserSeeder
    {
        public static User Me { get; set; } = new User()
        {
            Id = new Guid("60345AF2-506F-45D9-BC6D-1E5D16A0E105"),
            FirstName = "Harry",
            LastName = "Buxton",
            Email = "h.buxton@wearewattle.com",
            HashedPassword = "C4F4270B5689DA8BE30D0CD9BFD6BB3FF18E9EA08EDAC567CB2FBCCAFB21B0B5-3B1D5C556AF69604D16EB2C3B5821AB8",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static List<User> Generate()
        {
            return [ Me ];
        }
    }
}