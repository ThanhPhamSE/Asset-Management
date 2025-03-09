using System.ComponentModel.DataAnnotations;

namespace Asset_Management.Models
{
    public class AssetCheck
    {
        [Key]
        public int CheckId { get; set; }

        [Required]
        public int AssetId { get; set; }

        [Required]
        public int LocationId { get; set; }

        [Required]
        public DateTime CheckDate { get; set; }

        [MaxLength(100)]
        public string CheckedBy { get; set; }

        [Required]
        public int StatusId { get; set; }

        [MaxLength(255)]
        public string Notes { get; set; }

        public Asset Asset { get; set; }
        public Location Location { get; set; }
        public Status Status { get; set; }
    }
}
