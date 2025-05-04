using Microsoft.EntityFrameworkCore;
using ProductService.Models;
using ProductService.Dtos;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace ProductService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _dbContext;
        private readonly IDistributedCache _cache;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(ProductDbContext context, ILogger<ProductRepository> logger, IDistributedCache cache)
        {
            _dbContext = context;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Get records as per the filters supplied
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<(List<ProductSearchResult> Results, int TotalRecords)> SearchAsync(ProductSearchRequest request, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            // Construct the cache key from the request parameters
            string cacheKey = $"search:{JsonSerializer.Serialize(request)}:page:{pageNumber}:size:{pageSize}";

            try
            {
                // Check if data is available in the cache
                var cachedData = await _cache.GetStringAsync(cacheKey);
                if (cachedData != null)
                {
                    // Data found in cache, deserialize and return
                    var cachedResults = JsonSerializer.Deserialize<CachedSearchResult>(cachedData);
                    if (cachedResults?.Results.Count > 0)
                        return (cachedResults.Results, cachedResults.TotalRecords);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to access Redis cache for key: {CacheKey}", cacheKey);
            }

            var query = _dbContext.Items
            .AsNoTracking()
            .Include(i => i.Lab)
            .Include(i => i.Color)
            .Include(i => i.Type)
            .Include(i => i.Eligibility)
            .Include(i => i.Clarity)
            .Include(i => i.Cut)
            .Include(i => i.Polish)
            .Include(i => i.Symmetry)
            .Include(i => i.Fluorescence)
            .Include(i => i.Location)
            .AsQueryable();

            query = query
                .WhereIfIn(request.LabNames, i => i.Lab.Name)
                .WhereIfIn(request.ColorNames, i => i.Color.Name)
                .WhereIfIn(request.TypeNames, i => i.Type.Name)
                .WhereIfIn(request.EligibilityNames, i => i.Eligibility.Name)
                .WhereIfIn(request.ClarityNames, i => i.Clarity.Name)
                .WhereIfIn(request.CutNames, i => i.Cut.Name)
                .WhereIfIn(request.PolishNames, i => i.Polish.Name)
                .WhereIfIn(request.SymmetryNames, i => i.Symmetry.Name)
                .WhereIfIn(request.FluorescenceNames, i => i.Fluorescence.Name)
                .WhereIfIn(request.LocationNames, i => i.Location.Name);

            if (request.DiscountFrom.HasValue && request.DiscountFrom.Value > 0)
                query = query.Where(i => i.DiscountPercent >= request.DiscountFrom.Value);

            if (request.DiscountTo.HasValue && request.DiscountTo.Value > 0)
                query = query.Where(i => i.DiscountPercent <= request.DiscountTo.Value);

            if (request.PriceFrom.HasValue && request.PriceFrom.Value > 0)
                query = query.Where(i => i.FinalPrice >= request.PriceFrom.Value);

            if (request.PriceTo.HasValue && request.PriceTo.Value > 0)
                query = query.Where(i => i.FinalPrice <= request.PriceTo.Value);

            var totalRecords = await query.CountAsync(cancellationToken);

            string sortExpr = "ItemId ascending";
            if (!string.IsNullOrWhiteSpace(request.SortColumn))
                sortExpr = $"{request.SortColumn} {(request.SortDirection?.ToLower() == "desc" ? "descending" : "ascending")}";

            var results = await query
                .Select(i => new ProductSearchResult
                {
                    ItemId = i.ItemId,
                    ItemCode = i.ItemCode,
                    LabName = i.Lab != null ? i.Lab.Name : null,
                    ColorName = i.Color != null ? i.Color.Name : null,
                    TypeName = i.Type != null ? i.Type.Name : null,
                    ClarityName = i.Clarity != null ? i.Clarity.Name : null,
                    CutName = i.Cut != null ? i.Cut.Name : null,
                    PolishName = i.Polish != null ? i.Polish.Name : null,
                    SymmetryName = i.Symmetry != null ? i.Symmetry.Name : null,
                    FlourName = i.Fluorescence != null ? i.Fluorescence.Name : null,
                    LocationName = i.Location != null ? i.Location.Name : null,
                    BasePrice = i.BasePrice,
                    Discount = i.DiscountPercent,
                    Price = i.FinalPrice,
                })
                .OrderBy(sortExpr)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            try
            {
                // Cache the results for future use (cache for 1 hour)
                var cacheData = new CachedSearchResult() { Results = results, TotalRecords = totalRecords };
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(cacheData), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to access Redis cache for key: {CacheKey}", cacheKey);
            }

            return (results, totalRecords);
        }

        /// <summary>
        /// Test method to get records for test to see if data available 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<ProductSearchResult>> GetFirst10RecordsForTest(CancellationToken cancellationToken)
        {
            var results = await _dbContext.Items
                .AsNoTracking()
                .Include(i => i.Lab)
                .Include(i => i.Color)
                .Include(i => i.Type)
                .Include(i => i.Clarity)
                .Include(i => i.Cut)
                .Include(i => i.Polish)
                .Include(i => i.Symmetry)
                .Include(i => i.Fluorescence)
                .Include(i => i.Location)
                .OrderBy(i => i.ItemId)
                .Take(10)
                .Select(i => new ProductSearchResult
                {
                    ItemId = i.ItemId,
                    ItemCode = i.ItemCode,
                    LabName = i.Lab != null ? i.Lab.Name : null,
                    ColorName = i.Color != null ? i.Color.Name : null,
                    TypeName = i.Type != null ? i.Type.Name : null,
                    ClarityName = i.Clarity != null ? i.Clarity.Name : null,
                    CutName = i.Cut != null ? i.Cut.Name : null,
                    PolishName = i.Polish != null ? i.Polish.Name : null,
                    SymmetryName = i.Symmetry != null ? i.Symmetry.Name : null,
                    FlourName = i.Fluorescence != null ? i.Fluorescence.Name : null,
                    LocationName = i.Location != null ? i.Location.Name : null,
                    BasePrice = i.BasePrice,
                    Discount = i.DiscountPercent,
                    Price = i.FinalPrice
                })
                .ToListAsync(cancellationToken);

            return results;
        }

        /// <summary>
        /// Get all distinct values for the filters
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FilterOptions> GetAvailableFiltersAsync(CancellationToken cancellationToken)
        {
            string cacheKey = "filters:available-options";

            try
            {
                // Try get from cache
                var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);
                if (cachedData != null)
                {
                    return JsonSerializer.Deserialize<FilterOptions>(cachedData);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to access Redis cache for key: {CacheKey}", cacheKey);
            }

            var labs = await GetDistinctNonEmptyNamesAsync(_dbContext.Labs, i => i.Name, cancellationToken);
            var colors = await GetDistinctNonEmptyNamesAsync(_dbContext.Colors, i => i.Name, cancellationToken);
            var types = await GetDistinctNonEmptyNamesAsync(_dbContext.Types, i => i.Name, cancellationToken);
            var eligibilities = await GetDistinctNonEmptyNamesAsync(_dbContext.Eligibilities, i => i.Name, cancellationToken);
            var clarities = await GetDistinctNonEmptyNamesAsync(_dbContext.Clarities, i => i.Name, cancellationToken);
            var cuts = await GetDistinctNonEmptyNamesAsync(_dbContext.Cuts, i => i.Name, cancellationToken);
            var polishes = await GetDistinctNonEmptyNamesAsync(_dbContext.Polishes, i => i.Name, cancellationToken);
            var symmetries = await GetDistinctNonEmptyNamesAsync(_dbContext.Symmetries, i => i.Name, cancellationToken);
            var fluorescences = await GetDistinctNonEmptyNamesAsync(_dbContext.Fluorescences, i => i.Name, cancellationToken);
            var locations = await GetDistinctNonEmptyNamesAsync(_dbContext.Locations, i => i.Name, cancellationToken);

            var filters = new FilterOptions
            {
                Labs = labs,
                Colors = colors,
                Types = types,
                Eligibilities = eligibilities,
                Clarities = clarities,
                Cuts = cuts,
                Polishes = polishes,
                Symmetries = symmetries,
                Fluorescences = fluorescences,
                Locations = locations
            };

            try
            {
                // Cache the result (set 1 hour expiry — you can adjust)
                var serialized = JsonSerializer.Serialize(filters);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                };
                await _cache.SetStringAsync(cacheKey, serialized, options, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to set Redis cache for key: {CacheKey}", cacheKey);
            }
           
            return filters;
        }

        /// <summary>
        /// Generic method to get te distinct value from lookup tables
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="selector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<List<string>> GetDistinctNonEmptyNamesAsync<T>(
        IQueryable<T> dbSet,
        Expression<Func<T, string>> selector,
        CancellationToken cancellationToken)
        {
            return await dbSet
                .Select(selector)
                .Where(value => !string.IsNullOrEmpty(value))
                .Distinct()
                .ToListAsync(cancellationToken);
        }
    }
}