using Microsoft.Extensions.Caching.Distributed;
using ProductService.Dtos;
using System.Text.Json;

public class SearchCacheService : ISearchCacheService
{
    private readonly IDistributedCache _cache;
    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public SearchCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<List<ProductSearchResult>?> GetCachedResultsAsync(string cacheKey)
    {
        var cachedData = await _cache.GetStringAsync(cacheKey);
        if (string.IsNullOrEmpty(cachedData)) return null;

        return JsonSerializer.Deserialize<List<ProductSearchResult>>(cachedData, _jsonOptions);
    }

    public async Task SetCachedResultsAsync(string cacheKey, List<ProductSearchResult> results, TimeSpan expiry)
    {
        var jsonData = JsonSerializer.Serialize(results, _jsonOptions);
        await _cache.SetStringAsync(cacheKey, jsonData, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry
        });
    }
}
