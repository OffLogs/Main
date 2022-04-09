using System.Threading.Tasks;

namespace OffLogs.Web.Services.IO;

public interface IFilesCache
{
    Task<string> LoadAndCache(string filePath, string extension = "");
}
