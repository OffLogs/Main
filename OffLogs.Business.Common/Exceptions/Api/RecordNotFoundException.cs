using System;
using Domain.Abstractions;

namespace OffLogs.Business.Common.Exceptions.Api
{
    public class RecordNotFoundException : Exception, IDomainException
    {
        public RecordNotFoundException(string message = "Record was not found") : base(message)
        {
        }
    }
}
