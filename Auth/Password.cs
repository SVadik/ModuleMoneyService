using System;
using System.Security.Cryptography;
using System.Text;

namespace Auth
{
    public class Password
    {
        public Password(string password)
        {
            Salt = GetSalt();
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(string.Concat(password, Salt));
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                PasswordHash = sb.ToString();
            }
        }

        public string PasswordHash { get; }
        public string Salt { get; }

        public static bool CheckPassword(string password, string salt, string hash)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(string.Concat(password, salt));
                byte[] computedHash = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < computedHash.Length; i++)
                {
                    sb.Append(computedHash[i].ToString("X2"));
                }
                string computedHashString = sb.ToString();

                if (computedHashString == hash) return true;
                return false;
            }
        }

        private static string GetSalt(int maximumSaltLength = 32)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }
    }
}
