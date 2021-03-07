using System.Threading.Tasks;
using OffLogs.Console.Verbs;

namespace OffLogs.Console.Core
{
    public interface ICreateApplicationService
    {
        Task<int> Create(CreateNewApplicationVerb verb);
    }
}