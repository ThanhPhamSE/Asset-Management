using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Asset_Management.ViewModels
{
    public class UserViewModel
    {
        public string? Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
        //public bool IsActive { get; set; } = true; // Status

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string RoleId { get; set; }
        public List<RolesViewModel> Roles { get; set; } = new List<RolesViewModel>();

        [BindProperty]
        public string? IsActiveString { get; set; } = "true"; // Giá trị mặc định

        // Thuộc tính bool có getter và setter
        public bool IsActive
        {
            get => IsActiveString == "true"; // Chuyển đổi từ string sang bool
            set => IsActiveString = value ? "true" : "false"; // Chuyển đổi từ bool sang string
        }

    }
}
