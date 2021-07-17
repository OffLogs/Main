using System.Threading.Tasks;

namespace OffLogs.Business.Dao
{
    public interface ICommonDao
    {
        bool IsConnectionSuccessful();
        Task<bool> UpdateAsync(object entity);
        Task<long> InsertAsync(object entity);
    }
}