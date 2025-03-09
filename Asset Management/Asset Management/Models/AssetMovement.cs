using System.ComponentModel.DataAnnotations;

namespace Asset_Management.Models
{
    public class AssetMovement
    {
        [Key]
        public int MovementId { get; set; }

        [Required]
        public int AssetId { get; set; }

        [Required]
        public int FromLocationId { get; set; }

        [Required]
        public int ToLocationId { get; set; }

        [Required]
        public DateTime MoveDate { get; set; }

        [MaxLength(100)]
        public string ResponsiblePerson { get; set; }

        public Asset Asset { get; set; }
        public Location FromLocation { get; set; }
        public Location ToLocation { get; set; }
    }
}
