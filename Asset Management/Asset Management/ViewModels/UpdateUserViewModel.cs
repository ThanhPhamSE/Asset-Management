using System.ComponentModel.DataAnnotations;

namespace Asset_Management.ViewModels
{
    public class UpdateUserViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string RoleId { get; set; }
    }
}
