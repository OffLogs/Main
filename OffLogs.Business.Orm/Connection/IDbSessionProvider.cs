using System;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Connection
{
    public interface IDbSessionProvider: IDisposable, IExpectCommit
    {
        ISession CurrentSession { get; }
    }
}