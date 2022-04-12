using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using OffLogs.Web.Services.i18;

namespace OffLogs.Web.Services.IO;

public class FilesCache: IFilesCache
{
    private readonly string _baseUrl;
    private readonly HttpClient _httpClient;
    private readonly ILocalizationService _localizationService;
    private static readonly Dictionary<string, string> _filesCache = new();

    public FilesCache(
        HttpClient httpClient,
        IConfiguration configuration,
        ILocalizationService localizationService,
        NavigationManager navigationManager
    )
    {
        _httpClient = httpClient;
        _localizationService = localizationService;
        _baseUrl = navigationManager.BaseUri;
#if DEBUG
        _baseUrl = "http://localhost:5000/";
#endif
    }

    public async Task<string> LoadAndCache(string filePath, string extension = "")
    {
        var localePostfix = _localizationService.GetLocalePostfix();
        localePostfix = string.IsNullOrEmpty(localePostfix) ? "" : $".{localePostfix}";
        
        var extensionWithDot = string.IsNullOrEmpty(extension) ? "" : $".{extension}";
        var pathWithExtension = $"{filePath}{localePostfix}{extensionWithDot}";
        if (_filesCache.TryGetValue(pathWithExtension, out var fileContent))
        {
            return fileContent;
        }
        fileContent = await LoadFile(pathWithExtension);
        if (string.IsNullOrEmpty(fileContent))
        {
            var pathWithExtensionNonLocalized = $"{filePath}{extensionWithDot}";
            fileContent = await LoadFile(pathWithExtensionNonLocalized);
            if (string.IsNullOrEmpty(fileContent))
            {
                return null;    
            }
        }
        _filesCache.TryAdd(pathWithExtension, fileContent);
        return fileContent;
    }

    private async Task<string> LoadFile(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}{url}");
        var response = await _httpClient.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            return responseString;
        }

        return null;
    }
}
