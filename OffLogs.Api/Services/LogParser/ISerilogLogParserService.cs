using System.Threading.Tasks;
using OffLogs.Api.Models.Request.Log.Serilog;

namespace OffLogs.Api.Services.LogParser
{
    public interface ISerilogLogParserService
    {
        Task SaveAsync(SerilogEventsRequestModel model);
    }
}