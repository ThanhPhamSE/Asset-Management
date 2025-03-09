using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Asset_Management.Models
{
    public class AssetMaintenance
    {
        [Key]
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

        public Asset Asset { get; set; }
        public Status Status { get; set; }
    }
}
