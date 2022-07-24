using System;
using Domain.Abstractions;

namespace OffLogs.Business.Common.Exceptions.Api;

public class PaymentPackageRestrictionException : Exception, IDomainException
{
    public PaymentPackageRestrictionException(string message = "Payment package restriction") : base(message)
    {
    }
}
