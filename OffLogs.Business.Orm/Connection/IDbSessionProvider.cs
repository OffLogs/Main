using System;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;

namespace OffLogs.Business.Orm.Connection
{
    public interface IDbSessionProvider: IDisposable
    {
        ISession CurrentSession { get; }

        Task PerformCommit(CancellationToken cancellationToken = default);
    }
}