namespace Asset_Management.ViewModels
{
    public class AssetViewModel
    {
        public int AssetId { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public int CategoryId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal? CurrentValue { get; set; }
        public float? DepreciationRate { get; set; }
        public int StatusId { get; set; }
        public string? ImagePath { get; set; }  // Đường dẫn ảnh
        public IFormFile? ImageFile { get; set; } // File ảnh upload
        public int LocationId { get; set; }


        public string? CategoryName { get; set; }
        public string? LocationName { get; set; }
        public string? StatusName { get; set; }
    }
}
