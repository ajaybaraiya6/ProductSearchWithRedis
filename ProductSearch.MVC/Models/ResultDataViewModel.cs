namespace ProductSearch.MVC.Models
{
    public class ResultDataViewModel
    {
        public int ItemId { get; set; }
        public string? ItemCode { get; set; }
        public string? LabName { get; set; }
        public string? ColorName { get; set; }
        public string? TypeName { get; set; }
        public string? ClarityName { get; set; }
        public string? CutName { get; set; }
        public string? PolishName { get; set; }
        public string? SymmetryName { get; set; }
        public string? FlourName { get; set; }
        public string? LocationName { get; set; }
        public double? BasePrice { get; set; }
        public double? Discount { get; set; }
        public double? Price { get; set; }
    }
}
