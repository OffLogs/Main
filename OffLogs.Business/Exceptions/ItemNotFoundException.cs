using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Exceptions
{
    public class ItemNotFoundException : Exception, IDomainException
    {
        public ItemNotFoundException(string name) : base($"Item not found: {name}")
        {
        }
    }
}
