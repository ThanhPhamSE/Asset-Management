using System.ComponentModel.DataAnnotations;

namespace Asset_Management.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        [Required, MaxLength(100)]
        public string LocationName { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }
    }
}
