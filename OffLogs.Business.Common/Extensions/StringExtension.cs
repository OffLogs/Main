using System;
using System.Linq;
using System.Text;

namespace OffLogs.Business.Common.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input.First().ToString().ToUpper() + input.ToLower().Substring(1)
            };
        
        public static string Truncate(this string value, int maxLength, bool isAddDots = true)
        {
            if (string.IsNullOrEmpty(value)) 
                return value;
            var dots = isAddDots ? "..." : "";
            return value.Length <= maxLength ? value : $"{value[..maxLength]}${dots}"; 
        }
        
        public static string RemoveNewLines(this string value)
        {
            return value.Replace("\n", "")
                .Replace("\r", ""); 
        }
        
        public static byte[] GetUtf8Bytes(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }
    }
}
