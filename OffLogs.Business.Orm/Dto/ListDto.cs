using System.Collections;
using System.Collections.Generic;

namespace OffLogs.Business.Orm.Dto
{
    public class ListDto<T>
    {
        public ICollection<T> Items { get; }
        
        public long TotalCount { get; }

        public ListDto(ICollection<T> items, long count)
        {
            Items = items;
            TotalCount = count;
        }
    }
}