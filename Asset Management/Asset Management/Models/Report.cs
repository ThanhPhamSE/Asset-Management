using System.ComponentModel.DataAnnotations;

namespace Asset_Management.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        [Required, MaxLength(100)]
        public string ReportType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required, MaxLength(100)]
        public string GeneratedBy { get; set; }

        [Required]
        public DateTime DateGenerated { get; set; } = DateTime.UtcNow;
    }
}
