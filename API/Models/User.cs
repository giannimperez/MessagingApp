using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace API.Models
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateOfBirth { get; set; } // DateOnly datatype cannot be used as it isn't fully supported by entity framework
        public byte[] PaswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
