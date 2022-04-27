using System.Collections;
using System.Collections.Generic;

namespace OffLogs.Business.Orm.Dto
{
    public class ListDto<T>
    {
        public virtual ICollection<T> Items { get; set; }
        
        public virtual long TotalCount { get; set; }

        public ListDto()
        {
        }

        public ListDto(ICollection<T> items, long count)
        {
            Items = items;
            TotalCount = count;
        }
    }
}
