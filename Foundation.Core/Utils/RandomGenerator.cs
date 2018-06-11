using System;
using System.Security.Cryptography;
using System.Text;

namespace Foundation.Utils
{
    /// <summary>
    /// Utility class to generate random number/string.
    /// </summary>
    public static class RandomGenerator
    {
        /// <summary>
        /// Generate a random new number less than the specified max.
        /// </summary>
        /// <param name="max">Max.</param>
        /// <returns>A random number.</returns>
        public static int NewNumber(int max)
        {
            Random random = new Random();
            return random.Next(max);
        }

        /// <summary>
        /// Generate a new string with specified length.
        /// </summary>
        /// <param name="length">String length.</param>
        /// <returns>Generated string.</returns>
        public static string NewString(int length)
        {
            byte[] buffer = new byte[(length * 7 + 7) / 8];
            (new RNGCryptoServiceProvider()).GetBytes(buffer);

            return Convert.ToBase64String(buffer).Substring(0, length);
        }

        /// <summary>
        /// Compute has for the specified key and value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <returns>Hash.</returns>
        public static string ComputeHash(string key, string value)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(key + value);
            (new SHA1CryptoServiceProvider()).ComputeHash(buffer);

            return Convert.ToBase64String(buffer);
        }
    }
}