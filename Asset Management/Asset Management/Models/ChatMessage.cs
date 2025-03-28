using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_Management.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Text {  get; set; }
        public DateTime Date { get; set; }
        public string SenderId {  get; set; }
        public string ReceiverId {  get; set; }
        [ForeignKey(nameof(SenderId))]
        public Users Sender { get; set; }
        [ForeignKey(nameof(ReceiverId))]
        public Users Receiver { get; set; }
    }
}
