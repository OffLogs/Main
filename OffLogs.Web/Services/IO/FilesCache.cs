using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace OffLogs.Web.Services.IO;

public class FilesCache: IFilesCache
{
    private readonly string _baseUrl;
    private readonly HttpClient _httpClient;
    private static readonly Dictionary<string, string> _filesCache = new();

    public FilesCache(
        HttpClient httpClient,
        IConfiguration configuration
    )
    {
        _httpClient = httpClient;
#if DEBUG
        _baseUrl = "http://localhost:5000";
#else
        _baseUrl = configuration.GetValue<string>("ApiUrl");
#endif
    }

    public async Task<string> LoadAndCache(string url)
    {
        if (_filesCache.TryGetValue(url, out var fileContent))
        {
            return fileContent;
        }
        fileContent = await LoadFile(url);
        if (fileContent == null)
        {
            return null;
        }
        _filesCache.TryAdd(url, fileContent);
        return fileContent;
    }

    private async Task<string> LoadFile(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/{url}");
        var response = await _httpClient.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            return responseString;
        }

        return null;
    }
}
