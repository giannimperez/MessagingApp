using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class FriendshipDto
    {
        [Required]
        public string RequesterName { get; set; }
        [Required]
        public string AddresseeName { get; set; }
    }
}
