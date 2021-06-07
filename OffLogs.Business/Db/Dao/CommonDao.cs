using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OffLogs.Business.Db.Dao
{
    public class CommonDao: BaseDao, ICommonDao
    {
        public CommonDao(IConfiguration configuration, ILogger<CommonDao> logger) : base(
            configuration.GetConnectionString("DefaultConnection"), 
            logger
        )
        {
        }
        
        public bool IsConnectionSuccessful()
        {
            using (var session = Session)
            {
                return session.IsConnected;    
            }
        }
        
        public async Task<bool> UpdateAsync(object entity)
        {
            using (var session = Session)
            using(var transaction = session.BeginTransaction())
            {
                await session.UpdateAsync(entity);
                await transaction.CommitAsync();
                return true;
            }
        }
        
        public async Task<object> InsertAsync(object entity)
        {
            using (var session = Session)
            using(var transaction = session.BeginTransaction())
            {
                await session.SaveAsync(entity);
                await transaction.CommitAsync();
                return true;
            }
        }
    }
}