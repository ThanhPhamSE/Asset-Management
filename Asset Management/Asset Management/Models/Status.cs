using System.ComponentModel.DataAnnotations;

namespace Asset_Management.Models
{
    public class Status
    {
        [Key]
        public int StatusId { get; set; }

        [Required, MaxLength(50)]
        public string StatusName { get; set; }

    }
}
