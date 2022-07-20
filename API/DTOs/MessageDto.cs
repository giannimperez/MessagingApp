using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class MessageDto
    {
        [Required]
        public string Sender { get; set; }
        [Required]
        public string Recipient { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
