using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Text { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        
        public int ConversationId { get; set; }
    }
}