using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Utilities
{
    public class Cryptography
    {
        public static string Encrypt(string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text);
        }

        public static bool Verify(string text, string securedText)
        {
            return BCrypt.Net.BCrypt.Verify(text, securedText);
        }
    }
}
