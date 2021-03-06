using System.Threading.Tasks;
using OffLogs.Console.Verbs;

namespace OffLogs.Console.Core
{
    public interface ICreateUserService
    {
        Task<int> CreateUser(CreateNewUserVerb verb);
    }
}