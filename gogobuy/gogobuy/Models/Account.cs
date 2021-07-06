using System.Security.Cryptography;
using System.Text;

namespace gogobuy.Models
{
    public class Account
    {

        // 生成salt
        public static string GetSalt()
        {
            // 預設salt生成長度
            int maximumSaltLength = 32;

            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < salt.Length; i++)
            {
                sb.Append(salt[i].ToString("x2"));
            }

            return sb.ToString();
        }
        // 密碼加密
        public static string HashPassword(string plainText, string salt)
        {

            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes = System.Text.Encoding.ASCII.GetBytes(plainText + salt);
            byte[] hash = algorithm.ComputeHash(plainTextWithSaltBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }
        // 驗證密碼是否正確
        public static bool IsPasswordCorrect(string inputPassword, tMembership user)
        {
            string hashPassword = HashPassword(inputPassword, user.fSalt);
            if (hashPassword == user.fPassword)
                return true;

            return false;
        }
    }
}