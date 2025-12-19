using System;
using System.Security.Cryptography;

namespace Api.Infrastructure.Identity
{
    internal sealed class PasswordHasher : IPasswordHasher
    {
        public PasswordHasher()
        {
            SaltSize = 16;
            HashSize = 32;
            Iterations = 100_000;
            Algorithm = HashAlgorithmName.SHA512;
        }

        private int SaltSize { get; set; }
        private int HashSize { get; set; }
        private int Iterations { get; set; }
        private HashAlgorithmName Algorithm { get; set; }

        public string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

            var hexHash = Convert.ToHexString(hash);
            var hexSalt = Convert.ToHexString(salt);

            return $"{hexHash}-{hexSalt}";
        }
        
        public bool VerifyPassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split('-');
            var hash = Convert.FromHexString(parts[0]);
            var salt = Convert.FromHexString(parts[1]);

            var inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }
    }
}