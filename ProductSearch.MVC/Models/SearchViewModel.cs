namespace ProductSearch.MVC.Models
{
    public class SearchViewModel
    {
        public List<string>? LabNames { get; set; }
        public List<string>? ColorNames { get; set; }
        public List<string>? TypeNames { get; set; }
        public List<string>? EligibilityNames { get; set; }
        public List<string>? ClarityNames { get; set; }
        public List<string>? CutNames { get; set; }
        public List<string>? PolishNames { get; set; }
        public List<string>? SymmetryNames { get; set; }
        public List<string>? FluorescenceNames { get; set; }
        public List<string>? LocationNames { get; set; }

        public double? DiscountFrom { get; set; }
        public double? DiscountTo { get; set; }

        public double? PriceFrom { get; set; }
        public double? PriceTo { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
		public string? SortColumn { get; set; }
		public string? SortDirection { get; set; }
	}
}
