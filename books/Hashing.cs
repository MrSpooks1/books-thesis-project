using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace books
{
    public static class Hashing
    {
        public static string Hash(string input)
        {
            using (var sha = SHA1.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
