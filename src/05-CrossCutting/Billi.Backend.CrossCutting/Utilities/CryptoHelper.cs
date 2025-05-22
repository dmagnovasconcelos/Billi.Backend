using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Billi.Backend.CrossCutting.Utilities
{
    public static class CryptoHelper
    {
        public static string Encrypt(this string plainText, IConfiguration configuration)
        {
            if (string.IsNullOrWhiteSpace(plainText)) return plainText;

            var key = configuration["Crypto:Key"];
            var iv = configuration["Crypto:IV"];

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using var sw = new StreamWriter(cs);
            sw.Write(plainText);
            sw.Close();

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(this string cipherText, IConfiguration configuration)
        {
            if (string.IsNullOrWhiteSpace(cipherText)) return cipherText;

            var key = configuration["Crypto:Key"];
            var iv = configuration["Crypto:IV"];

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
