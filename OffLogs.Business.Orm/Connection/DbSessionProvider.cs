using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NHibernate;

namespace OffLogs.Business.Orm.Connection
{
    internal class DbSessionProvider : IDbSessionProvider
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<IDbConnectionFactory> _logger;

        private ISession _session { get; set; }
        private ISessionFactory _sessionFactory { get; set; }

        public ISession CurrentSession {
            get {
                if (_session == null || !_session.IsOpen)
                {
                    _session = _sessionFactory.OpenSession();
                    _transaction = _session.BeginTransaction();
                }
                return _session;
            }
        }

        private ITransaction _transaction;

        public DbSessionProvider(
            IDbConnectionFactory dbConnectionFactory, 
            ILogger<IDbConnectionFactory> logger
        )
        {
            this._dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sessionFactory = _dbConnectionFactory.GetSessionFactoryAsync().Result;
        }

        ~DbSessionProvider()
        {
            //Dispose();
        }

        public async Task PerformCommit(CancellationToken cancellationToken = default)
        {
            if (_transaction != null && _session.IsOpen)
            {
                try
                {
                    await _transaction.CommitAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message, e);
                    await _transaction.RollbackAsync();
                    throw e;
                }
            }
            _transaction?.Dispose();
            _transaction = null;
        }

        #region IDisposable implementation
        public void Dispose()
        {
            if (_transaction != null && _transaction.IsActive)
            {
                PerformCommit().Wait();
            }
            if (_session != null)
            {
                _session?.Dispose();
                _session = null;
            }
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}