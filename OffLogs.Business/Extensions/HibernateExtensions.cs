using System.Collections;
using System.Collections.Generic;
using NHibernate.Collection;

namespace OffLogs.Business.Extensions
{
    public static class HibernateExtensions
    {
        public static bool IsHibernateLazy(this ICollection collection)
        {
            if (collection is ILazyInitializedCollection)
            {
                return true;
            }
            return false;
        }
        
        public static bool IsHibernateLazy<T>(this ICollection<T> collection)
        {
            if (collection is ILazyInitializedCollection)
            {
                return true;
            }
            return false;
        }
    }
}