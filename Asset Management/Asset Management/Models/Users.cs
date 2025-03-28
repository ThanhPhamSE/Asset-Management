using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_Management.Models
{
    public class Users : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [InverseProperty(nameof(ChatMessage.Sender))]
        public virtual ICollection<ChatMessage> SentMessages { get; set; }
        [InverseProperty(nameof (ChatMessage.Receiver))]
        public virtual ICollection<ChatMessage> ReceivedMessages { get; set; }
    }
}
