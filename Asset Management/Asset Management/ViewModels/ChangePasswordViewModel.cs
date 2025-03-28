using System.ComponentModel.DataAnnotations;

namespace Asset_Management.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Token { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is required.")]
        [StringLength(40, MinimumLength =8,ErrorMessage ="The {0} must be at {2} and max {1} character")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [Compare("ConfirmNewPassword", ErrorMessage ="Confirm Password is required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage ="Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        public string ConfirmNewPassword { get; set; }
    }
}
