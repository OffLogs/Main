using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OffLogs.Business.Dao
{
    public class CommonDao: BaseDao, ICommonDao
    {
        public CommonDao(IConfiguration configuration, ILogger<CommonDao> logger) : base(
            configuration, 
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
        
        public async Task<long> InsertAsync(object entity)
        {
            using (var session = Session)
            using(var transaction = session.BeginTransaction())
            {
                var id = await session.SaveAsync(entity);
                await transaction.CommitAsync();
                return (long?) id ?? 0;
            }
        }
    }
}