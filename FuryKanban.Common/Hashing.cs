using System;
using System.Security.Cryptography;

namespace FuryKanban.Common
{
    public static class Hashing
    {
        #region SHA1
        public static string GetSHA1(byte[] data)
        {
            SHA1 shaHasher = SHA1.Create();
            byte[] hash = shaHasher.ComputeHash(data);

            return BitConverter.ToString(hash).Replace("-", "");
        }
        public static string GetSHA1(string data)
        {
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(data);
            return GetSHA1(byteData);
        }
        #endregion
        public static string GetPasswordHash(string password, string salt)
        {
            return Hashing.GetSHA1(password + Hashing.GetSHA1(salt));
        }
    }
}