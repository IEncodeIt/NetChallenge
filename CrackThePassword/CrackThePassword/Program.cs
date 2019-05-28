using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrackThePassword
{
    class Program
    {
        private static SHA256Managed Sha256;

        static void Main(string[] args)
        {
            string hash = String.Empty;
            int password = 0;
            string userHash = "d887b8314484a54a1aafca4da2fa542ac3d8ff8cdf51625db97b15bd5c098cf0";

            Sha256 = new SHA256Managed();

            do
            {
                password++;
                hash = GetHash(password.ToString());
            } while (hash != userHash);

            Console.WriteLine(password);
            Console.ReadKey();
        }

        //Считает хэш любой строки
        public static string GetHash(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            byte[] hash = Sha256.ComputeHash(bytes);
            StringBuilder hashStringBuilder = new StringBuilder(64);

            foreach (byte x in hash)
            {
                hashStringBuilder.Append(string.Format("{0:x2}", x));
            }

            return hashStringBuilder.ToString();
        }
    }
}
