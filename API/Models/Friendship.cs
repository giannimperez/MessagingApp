using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Friendship
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("RequesterId")]
        public User Requester { get; set; }

        [Required]
        [ForeignKey("AddresseeId")]
        public User Addressee { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
}
