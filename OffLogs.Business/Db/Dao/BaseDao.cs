using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;

namespace OffLogs.Business.Db.Dao
{
    public class BaseDao: IDisposable
    {
        protected ILogger<BaseDao> Logger;
        
        protected SqlConnection Connection
        {
            get
            {
                OpenConnection();
                return _connection;
            }
        }

        private readonly SqlConnection _connection;

        #region CommonMethods

        public BaseDao(string connString, ILogger<BaseDao> logger)
        {
            Logger = logger;
            _connection = new SqlConnection(connString);
            OpenConnection();
        }

        public void OpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
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

        public SqlConnection GetConnection()
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

        protected long Insert(object entity)
        {
            return Connection.Insert(entity);
        }
        
        protected async Task<int> InsertAsync(object entity)
        {
            return await Connection.InsertAsync(entity);
        }
        
        #endregion
    }
}