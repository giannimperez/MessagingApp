using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Password { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}