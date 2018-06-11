namespace Foundation.Utils
{
    /// <summary>
    /// Utility class that implements salt & hash algorithem.
    /// </summary>
    public sealed class SaltedHash
    {
        private const int _saltLength = 6;

        private string _salt;
        private string _hash;

        /// <summary>
        /// Construct a SaltedHash.
        /// </summary>
        /// <param name="salt">Salt.</param>
        /// <param name="hash">Hash.</param>
        private SaltedHash(string salt, string hash)
        {
            _salt = salt;
            _hash = hash;
        }

        /// <summary>
        /// Create a SaltedHash instance with specified password.
        /// </summary>
        /// <param name="password">Password.</param>
        /// <returns>SaltedHash instance.</returns>
        public static SaltedHash Create(string password)
        {
            string salt = NewSalt();
            string hash = ComputeHash(salt, password);

            return new SaltedHash(salt, hash);
        }

        /// <summary>
        /// Create a SaltedHash instance with specified salt and hash.
        /// </summary>
        /// <param name="salt">Salt.</param>
        /// <param name="hash">Hash.</param>
        /// <returns>SaltedHash instance.</returns>
        public static SaltedHash Create(string salt, string hash)
        {
            return new SaltedHash(salt, hash);
        }

        /// <summary>
        /// Salt.
        /// </summary>
        public string Salt
        {
            get
            {
                return _salt;
            }
        }

        /// <summary>
        /// Hash.
        /// </summary>
        public string Hash
        {
            get
            {
                return _hash;
            }
        }

        /// <summary>
        /// Verify if the specified password matches with current hash.
        /// </summary>
        /// <param name="password">Password.</param>
        /// <returns>If the password matches.</returns>
        public bool Verify(string password)
        {
            return _hash.Equals(ComputeHash(_salt, password));
        }

        /// <summary>
        /// Create a new salt.
        /// </summary>
        /// <returns>New salt.</returns>
        private static string NewSalt()
        {
            return RandomGenerator.NewString(_saltLength);
        }

        /// <summary>
        /// Compute hash for the specified salt and password.
        /// </summary>
        /// <param name="salt">Salt.</param>
        /// <param name="password">Password.</param>
        /// <returns>Hash.</returns>
        private static string ComputeHash(string salt, string password)
        {
            return RandomGenerator.ComputeHash(salt, password);
        }
    }
}