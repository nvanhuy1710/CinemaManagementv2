using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace Cinema.Helper
{
    public class HashPassword
    {

        public static byte[] salt = Encoding.UTF8.GetBytes("Thisissecretkeyforhashpassword");

        public static string HashByPBKDF2(string password)
        {
            // Generate a 128-bit salt using a sequence of
            // cryptographically strong random bytes.
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
