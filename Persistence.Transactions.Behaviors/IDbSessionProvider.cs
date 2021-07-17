using System;
using NHibernate;

namespace Persistence.Transactions.Behaviors
{
    public interface IDbSessionProvider: IDisposable, IExpectCommit
    {
        ISession CurrentSession { get; }
    }
}