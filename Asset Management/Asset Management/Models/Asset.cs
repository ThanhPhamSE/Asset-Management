using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Asset_Management.Models
{
    public class Asset
    {
        [Key]
        public int AssetId { get; set; }

        [Required, MaxLength(50)]
        public string AssetCode { get; set; }

        [Required, MaxLength(100)]
        public string AssetName { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public decimal PurchasePrice { get; set; }

        public decimal? CurrentValue { get; set; }

        [Range(0, 100)]
        public float? DepreciationRate { get; set; }

        [Required]
        public int StatusId { get; set; }

        [MaxLength(255)]
        public string ImageUrl { get; set; }

        [Required]
        public int LocationId { get; set; }

        public Category Category { get; set; }
        public Status Status { get; set; }
        public Location Location { get; set; }

    }
}
