using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Echo
{
    public static class Security
    {
        public static string ConnectionString = "Server=MYSQL6001.site4now.net;Database=db_a45ae3_news;Uid=a45ae3_news;Pwd=adanijela1;pooling=false";
        public static string HashPassword(string RawPassword)
        {
            try
            {
                return hash256(hash256(hash256(hash256(hash256(hash256(hash256(hash256(RawPassword))))))));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool HasInvalidCharacters(this string sender, bool SkipSpace = false)
        {
            if (string.IsNullOrWhiteSpace(sender) || sender.Length == 0)
                return true;

            foreach (char c in sender)
                if (!Char.IsNumber(c) && !Char.IsLetter(c))
                {
                    if (c == ' ')
                    {
                        if (!SkipSpace)
                            return true;
                    }
                    else
                        return true;
                }

            return false;
        }


        private static string hash256(string Raw)
        {

            try
            {
                // Create a SHA256   
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    // ComputeHash - returns byte array  
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(Raw));

                    // Convert byte array to a string   
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
