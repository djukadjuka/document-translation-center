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
    }
}
