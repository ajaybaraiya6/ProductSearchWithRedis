using ProductSearch.MVC.Models;

public class SearchResultViewModel
{
	public List<ResultDataViewModel> Data { get; set; } = new();
	public int TotalRecords { get; set; }
	public int PageNumber { get; set; }
	public int PageSize { get; set; }
}
