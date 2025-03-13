using System.ComponentModel.DataAnnotations;

namespace Asset_Management.ViewModels
{
    public class AssetMovementViewModel
    {
        public int MovementId { get; set; }
        [Required(ErrorMessage = "Asset is required.")]
        public string AssetCode { get; set; }
        public string? AssetName { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public DateTime MoveDate { get; set; }
        [Required(ErrorMessage = "Responsible Person is required.")]
        public string ResponsiblePerson { get; set; }
    }
}
