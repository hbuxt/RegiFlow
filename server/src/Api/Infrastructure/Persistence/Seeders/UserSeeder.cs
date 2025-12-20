using System;
using System.Collections.Generic;
using Api.Domain.Entities;

namespace Api.Infrastructure.Persistence.Seeders
{
    internal static class UserSeeder
    {
        private const string RawPassword = "P4ssw0rd1!";
        private const string HashedPassword = "211D96B96501A67DF5A3133554554F8BA526F66DD2185CE9E3EDA6C6D51A4DC5-49845E168BF607D5952C745D18899D09";
        
        public static User DefaultAdministrator { get; set; } = new User()
        {
            Id = new Guid("60345AF2-506F-45D9-BC6D-1E5D16A0E105"),
            FirstName = "Harry",
            LastName = "Buxton",
            Email = "h.buxton@wearewattle.com",
            HashedPassword = "C4F4270B5689DA8BE30D0CD9BFD6BB3FF18E9EA08EDAC567CB2FBCCAFB21B0B5-3B1D5C556AF69604D16EB2C3B5821AB8",
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static User DemoUser1 { get; set; } = new User()
        {
            Id = new Guid("9F6BB990-EDB9-41FC-8F5E-EC63FA3E4D3F"),
            FirstName = "Jamie",
            LastName = "Patel",
            Email = "jamie.patel@example.com",
            HashedPassword = HashedPassword,
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static User DemoUser2 { get; set; } = new User()
        {
            Id = new Guid("AA5BED02-852A-4A88-A107-159FBAB169C2"),
            FirstName = "Taylor",
            LastName = "Nguyen",
            Email = "taylor.nguyen@example.com",
            HashedPassword = HashedPassword,
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static User DemoUser3 { get; set; } = new User()
        {
            Id = new Guid("CDD9D41A-161E-4DD5-A9F4-D521233F03B7"),
            FirstName = "Chris",
            LastName = "Reynolds",
            Email = "chris.reynolds@example.com",
            HashedPassword = HashedPassword,
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static User DemoUser4 { get; set; } = new User()
        {
            Id = new Guid("929BFD3F-148A-4E2E-85D7-411DE090EBFE"),
            FirstName = "Jordan",
            LastName = "Alvarez",
            Email = "jordan.alvarez@example.com",
            HashedPassword = HashedPassword,
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };
        
        public static User DemoUser5 { get; set; } = new User()
        {
            Id = new Guid("228D0A28-FE99-4C14-9E4C-D43B5FEB02ED"),
            FirstName = "Alex",
            LastName = "Morgan",
            Email = "alex.morgan@example.com",
            HashedPassword = HashedPassword,
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
        };

        public static List<User> Generate()
        {
            return [ DefaultAdministrator, DemoUser1, DemoUser2, DemoUser3, DemoUser4, DemoUser5 ];
        }
    }
}