using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace API.Models
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime DateOfBirth { get; set; } = DateTime.Today;
        [JsonIgnore]
        public byte[] PaswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public int GetAge()
        {
            var currentDate = DateTime.Today;
            var age = currentDate.Year - DateOfBirth.Year;
            return age;
        }
    }
}
