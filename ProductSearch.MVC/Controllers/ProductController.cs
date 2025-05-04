using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ProductSearch.MVC.Models;

namespace ProductSearch.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProductController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Search()
        {
            var client = _clientFactory.CreateClient(Constants.ProductApiClientName);
            var filters = await client.GetFromJsonAsync<FilterOptionsViewModel>("filters");

            ViewBag.FilterOptions = filters;

            return View(new SearchViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Search(SearchViewModel search)
        {
            var client = _clientFactory.CreateClient(Constants.ProductApiClientName);
            var response = await client.PostAsJsonAsync("search", search);

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            var result = await response.Content.ReadFromJsonAsync<SearchResultViewModel>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var resultData = result?.Data ?? new List<ResultDataViewModel>();
            ViewBag.TotalRecords = result?.TotalRecords;

            return View("Result", (resultData, search));
        }

    }
}
