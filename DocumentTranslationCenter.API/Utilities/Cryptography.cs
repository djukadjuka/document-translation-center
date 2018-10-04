using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DocumentTranslationCenter.API.Utilities
{
    public class Cryptography
    {
        /// <summary>
        /// Creates a random salt in the form of a base64 string.
        /// Use Convert.FromBase64String(salt) to get the bytes before putting in algorithm for hashing ...
        /// </summary>
        /// <returns>Base 64 string representation of random 128 bits of data.</returns>
        public static string Get128BitBase64Salt()
        {
            byte[] salt = new byte[128 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Hashes a password using base64 salt, 10000 iterations and 32 bytes
        /// </summary>
        /// <param name="password">The password that needs to be hashed.</param>
        /// <param name="salt">The salt used for hashing a password.</param>
        /// <returns>Base64 string of the hashed password.</returns>
        public static string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            string hashedPassword = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                        password: password,
                        salt: saltBytes,
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 10000,
                        numBytesRequested: 256 / 8
                    )
                );
            return hashedPassword;
        }
    }
}
