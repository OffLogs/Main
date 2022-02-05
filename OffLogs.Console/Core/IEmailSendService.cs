using System.Threading.Tasks;
using OffLogs.Console.Verbs;

namespace OffLogs.Console.Core
{
    public interface IEmailSendService
    {
        Task EmailSend(EmailSendVerb verb);
    }
}
