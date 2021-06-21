﻿using System.Threading;
using System.Threading.Tasks;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Business.Services.Communication
{
    public interface IKafkaConsumerService
    {
        Task<long> ProcessLogsAsync(int? millisecondsTimeout = null, CancellationToken cancellationToken = default);
    }
}