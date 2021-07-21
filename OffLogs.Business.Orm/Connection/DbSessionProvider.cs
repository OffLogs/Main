﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NHibernate;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Connection
{
    public class DbSessionProvider : IDbSessionProvider
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
                }
                if (_transaction == null || !_transaction.IsActive)
                {
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

        public async Task PerformCommitAsync(CancellationToken cancellationToken = default)
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
        
        public async Task RefreshEntityAsync(object entity, CancellationToken cancellationToken = default)
        {
            if (_session != null && _session.IsOpen)
            {
                try
                {
                    await _session.RefreshAsync(entity, cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message, e);
                    throw;
                }
            }
        }
        
        #region IDisposable implementation
        public void Dispose()
        {
            if (_transaction != null && _transaction.IsActive)
            {
                PerformCommitAsync().Wait();
            }
            if (_session != null)
            {
                _session.Dispose();
                _session = null;
            }
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}