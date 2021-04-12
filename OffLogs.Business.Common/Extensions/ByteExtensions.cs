using System;

namespace OffLogs.Business.Common.Extensions
{
    public static class ByteExtensions
    {
        /// <summary>  
        ///  Returns true if arrays are equals
        /// </summary>  
        public static Boolean CompareTo(this byte[] bytes, byte[] toCompare)
        {
            if (bytes == null || toCompare == null)
            {
                return false;
            }
            if (toCompare.Length != bytes.Length)
                return false;

            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] != toCompare[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}