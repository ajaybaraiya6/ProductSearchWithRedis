using ProductService.Dtos;

public class CachedSearchResult
{
    public List<ProductSearchResult> Results { get; set; }
    public int TotalRecords { get; set; }
}