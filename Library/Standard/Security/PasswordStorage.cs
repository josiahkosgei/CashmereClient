
// Type: Cashmere.Library.Standard.Security.PasswordStorage


using System;
using System.Security.Cryptography;

namespace Cashmere.Library.Standard.Security
{
    public class PasswordStorage
    {
        public const int SALT_BYTES = 24;
        public const int HASH_BYTES = 18;
        public const int PBKDF2_ITERATIONS = 64000;
        public const int HASH_SECTIONS = 5;
        public const int HASH_ALGORITHM_INDEX = 0;
        public const int ITERATION_INDEX = 1;
        public const int HASH_SIZE_INDEX = 2;
        public const int SALT_INDEX = 3;
        public const int PBKDF2_INDEX = 4;

        public static string CreateHash(string password)
        {
            byte[] numArray = new byte[24];
            try
            {
                using RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider();
                cryptoServiceProvider.GetBytes(numArray);
            }
            catch (CryptographicException ex)
            {
                throw new CannotPerformOperationException("Random number generator not available.", ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException("Invalid argument given to random number generator.", ex);
            }
            byte[] inArray = PBKDF2(password, numArray, 64000, 18);
            return "sha1:" + 64000 + ":" + inArray.Length + ":" + Convert.ToBase64String(numArray) + ":" + Convert.ToBase64String(inArray);
        }

        public static bool VerifyPassword(string password, string goodHash)
        {
            if (password == null)
                return goodHash == null;
            char[] chArray = new char[1] { ':' };
            string[] strArray = goodHash.Split(chArray);
            if (strArray.Length != 5)
                throw new InvalidHashException("Fields are missing from the password hash.");
            if (strArray[0] != "sha1")
                throw new CannotPerformOperationException("Unsupported hash type.");
            int iterations;
            try
            {
                iterations = int.Parse(strArray[1]);
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException("Invalid argument given to Int32.Parse", ex);
            }
            catch (FormatException ex)
            {
                throw new InvalidHashException("Could not parse the iteration count as an integer.", ex);
            }
            catch (OverflowException ex)
            {
                throw new InvalidHashException("The iteration count is too large to be represented.", ex);
            }
            if (iterations < 1)
                throw new InvalidHashException("Invalid number of iterations. Must be >= 1.");
            byte[] salt;
            try
            {
                salt = Convert.FromBase64String(strArray[3]);
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException("Invalid argument given to Convert.FromBase64String", ex);
            }
            catch (FormatException ex)
            {
                throw new InvalidHashException("Base64 decoding of salt failed.", ex);
            }
            byte[] a;
            try
            {
                a = Convert.FromBase64String(strArray[4]);
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException("Invalid argument given to Convert.FromBase64String", ex);
            }
            catch (FormatException ex)
            {
                throw new InvalidHashException("Base64 decoding of pbkdf2 output failed.", ex);
            }
            int num;
            try
            {
                num = int.Parse(strArray[2]);
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException("Invalid argument given to Int32.Parse", ex);
            }
            catch (FormatException ex)
            {
                throw new InvalidHashException("Could not parse the hash size as an integer.", ex);
            }
            catch (OverflowException ex)
            {
                throw new InvalidHashException("The hash size is too large to be represented.", ex);
            }
            if (num != a.Length)
                throw new InvalidHashException("Hash length doesn't match stored hash length.");
            byte[] b = PBKDF2(password, salt, iterations, a.Length);
            return SlowEquals(a, b);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            int num = a.Length ^ b.Length;
            for (int index = 0; index < a.Length && index < b.Length; ++index)
                num |= a[index] ^ b[index];
            return num == 0;
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            using Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt);
            rfc2898DeriveBytes.IterationCount = iterations;
            return rfc2898DeriveBytes.GetBytes(outputBytes);
        }
    }
}
