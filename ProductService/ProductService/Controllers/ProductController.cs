using Microsoft.AspNetCore.Mvc;
using ProductService.Repositories;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductRepository repository, ILogger<ProductController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    private (int PageNumber, int PageSize) GetValidatedPagination(int? pageNumber, int? pageSize, int maxPageSize = 100)
    {
        var page = pageNumber > 0 ? pageNumber.Value : 1;
        var size = pageSize > 0 ? pageSize.Value : 10;
        size = Math.Min(size, maxPageSize);
        return (page, size);
    }

    /// <summary>
    /// Search API to get the records based on filters supplied
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] ProductSearchRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var (pageNumber, pageSize) = GetValidatedPagination(request.PageNumber, request.PageSize);
            var (results, totalRecords) = await _repository.SearchAsync(request, pageNumber, pageSize, cancellationToken);

            return Ok(new
            {
                data = results,
                totalRecords,
                pageNumber,
                pageSize,
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Product search failed: {@Request}", request);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error processing request", details = ex.Message });
        }
    }

    /// <summary>
    /// Helper get filter options method to get distinct lookup values from data base for front end
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("filters")]
    public async Task<IActionResult> GetFilterOptions(CancellationToken cancellationToken)
    {
        var filters = await _repository.GetAvailableFiltersAsync(cancellationToken);

        return Ok(filters);
    }

    /// <summary>
    /// Test method to check if our database file contain any records
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("test")]
    public async Task<IActionResult> GetFirst10RecordsForTest(CancellationToken cancellationToken)
    {
        var results = await _repository.GetFirst10RecordsForTest(cancellationToken);
        return Ok(results);
    }
}
