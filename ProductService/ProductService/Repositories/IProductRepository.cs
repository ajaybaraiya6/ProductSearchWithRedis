using ProductService.Dtos;

namespace ProductService.Repositories
{
    public interface IProductRepository
    {
        Task<(List<ProductSearchResult> Results, int TotalRecords)> SearchAsync(ProductSearchRequest request, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<List<ProductSearchResult>> GetFirst10RecordsForTest(CancellationToken cancellationToken);
        Task<FilterOptions> GetAvailableFiltersAsync(CancellationToken cancellationToken);
    }
}
