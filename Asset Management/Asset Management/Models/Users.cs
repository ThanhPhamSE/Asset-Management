using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Asset_Management.Models
{
    public class Users : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

    }
}
