using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Asset_Management.ViewModels
{
    public class RoleViewModel
    {
        public string? Id { get; set; } // Cho phép null

        [Required(ErrorMessage = "Role Name is required")]
        public string Name { get; set; }
    }
}
