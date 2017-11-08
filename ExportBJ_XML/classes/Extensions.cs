using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExportBJ_XML.classes
{
    static class Extensions
    {
        public static bool In<T>(this T item, params T[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            return items.Contains(item);
        }
        public static bool IsBetween<T>(this T item, T start, T end)
        {
            return Comparer<T>.Default.Compare(item, start) >= 0
                && Comparer<T>.Default.Compare(item, end) < 0;
        }

        public static string XmlCharacterWhitelist(string in_string)
        {
            if (in_string == null) return null;

            StringBuilder sbOutput = new StringBuilder();
            char ch;

            for (int i = 0; i < in_string.Length; i++)
            {
                ch = in_string[i];
                if ((ch >= 0x0020 && ch <= 0xD7FF) ||
                    (ch >= 0xE000 && ch <= 0xFFFD) ||
                    ch == 0x0009 ||
                    ch == 0x000A ||
                    ch == 0x000D)
                {
                    sbOutput.Append(ch);
                }
            }
            return sbOutput.ToString();
        }

        public static string sha256(string randomString)
        {
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
            System.Text.StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString), 0, Encoding.UTF8.GetByteCount(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

    }
}
