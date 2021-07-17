using System;
using System.Threading.Tasks;
using NHibernate;

namespace OffLogs.Business.Orm.Connection
{
    public interface IDbConnectionFactory: IDisposable
    {
        Task<ISessionFactory> GetSessionFactoryAsync();
    }
}