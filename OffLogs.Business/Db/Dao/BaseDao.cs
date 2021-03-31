using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Npgsql;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;

namespace OffLogs.Business.Db.Dao
{
    public class BaseDao: IDisposable
    {
        protected ILogger<BaseDao> Logger;
        private static ConcurrentDictionary<string, string> _queryFilesCache = new();

        private OrmLiteConnectionFactory _connectionFactory;
        
        protected IDbConnection Connection
        {
            get
            {
                OpenConnection();
                return _connection;
            }
        }

        private IDbConnection _connection;

        #region CommonMethods

        public BaseDao(string connString, ILogger<BaseDao> logger)
        {
            Logger = logger;
            _connectionFactory= new OrmLiteConnectionFactory(connString, PostgreSqlDialect.Provider);
        }

        public void OpenConnection()
        {
            if (_connection == null)
            {
                _connection = _connectionFactory.Open();
            } 
            else if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (_connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }
        }


        public bool IsConnectionOpened()
        {
            return _connection.State == ConnectionState.Open;
        }

        public Boolean IsClosed()
        {
            return _connection == null || _connection.State == ConnectionState.Closed;
        }

        public void Dispose()
        {
            _connection?.Dispose(); // this will also CLOSE connection if Open

            // Use SupressFinalize in case a subclass 
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        public IDbConnection GetConnection()
        {
            return Connection;
        }
        
        public int ExecuteWithReturn(string sprName, DynamicParameters param = null)
        {
            param ??= new DynamicParameters();
            param.Add("@ret", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            Connection.Execute(sprName, param, null, null, CommandType.StoredProcedure);
            return param.Get<int>("ret");
        }

        public async Task<int> ExecuteWithReturnAsync(string sprName, object param = null)
        {
            return await ExecuteWithReturnAsync(sprName, new DynamicParameters(param));
        }

        public async Task<int> ExecuteWithReturnAsync(string sprName, DynamicParameters param = null)
        {
            param ??= new DynamicParameters();
            param.Add("@ret", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            await Connection.ExecuteAsync(sprName, param, null, null, CommandType.StoredProcedure);
            return param.Get<int>("ret");
        }
        
        #endregion

        #region QueryReader

        public string GetQuery(string fileName)
        {
            if (_queryFilesCache.ContainsKey(fileName))
            {
                return _queryFilesCache[fileName];
            }

            var queryString = LoadDbQueryFile(fileName);
            _queryFilesCache.TryAdd(queryString, fileName);
            return queryString;
        }

        private string LoadDbQueryFile(string fileName)
        {
            var assembly = GetType().Assembly;
            var name = assembly.GetName();
            var resource = assembly.GetManifestResourceStream($"{name.Name}.Db.Queries.{fileName}.sql");
            if (resource == null)
            {
                throw new Exception($"Query file wasn't found: '{fileName}'");
            }
            using (var reader = new StreamReader(resource))
            {
                return reader.ReadToEnd();
            }
        }

        #endregion
    }
}