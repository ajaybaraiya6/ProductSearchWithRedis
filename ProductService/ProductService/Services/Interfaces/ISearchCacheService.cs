using ProductService.Dtos;

public interface ISearchCacheService
{
    Task<List<ProductSearchResult>?> GetCachedResultsAsync(string cacheKey);
    Task SetCachedResultsAsync(string cacheKey, List<ProductSearchResult> results, TimeSpan expiry);
}
