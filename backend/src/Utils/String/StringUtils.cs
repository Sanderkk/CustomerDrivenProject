using System;
using System.Text;

namespace src.Utils
{
    public class StringUtils
    {
        
        public static string RemoveWhiteSpaces(string str)
        {
            /*
            Fast code to remove whitespace

            :param string with whitespace
            :return string without whitespace
            */

            var sb = new StringBuilder();
            if (str == null) return sb.ToString();
            foreach (var c in str)
            {
                if (!char.IsWhiteSpace(c))
                    sb.Append(c);
            }

            return sb.ToString();
        }
        
    }
    
}