using System;
using System.Threading.Tasks;
using NHibernate;

namespace OffLogs.Business.Orm.Connection
{
    internal interface IDbConnectionFactory: IDisposable
    {
        Task<ISessionFactory> GetSessionFactoryAsync();
    }
}