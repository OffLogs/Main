using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Services
{
    public interface IConfigurationService: IDomainService
    {
        public string SupportEmail { get; }
    }
}
