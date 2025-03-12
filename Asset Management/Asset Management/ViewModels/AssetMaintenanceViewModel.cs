using System.ComponentModel.DataAnnotations;

namespace Asset_Management.ViewModels
{
    public class AssetMaintenanceViewModel
    {
        public int MaintenanceId { get; set; }

        [Required]
        public int AssetId { get; set; }

        [Required]
        public DateTime MaintenanceDate { get; set; }

        [Required, MaxLength(100)]
        public string MaintenanceType { get; set; }

        public decimal? MaintenanceCost { get; set; }

        [Required]
        public int StatusId { get; set; }

        [MaxLength(255)]
        public string Notes { get; set; }

        public string? AssetName { get; set; } // Thêm tên tài sản
        public string? StatusName { get; set; } // Thêm tên trạng thái
    }
}
