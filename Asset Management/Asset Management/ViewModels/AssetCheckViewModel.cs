namespace Asset_Management.ViewModels
{
    public class AssetCheckViewModel
    {
        public int CheckId { get; set; }
        public int AssetId { get; set; }
        public string? AssetName { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public DateTime CheckDate { get; set; }
        public string CheckedBy { get; set; }
        public string Notes { get; set; }
    }
}
