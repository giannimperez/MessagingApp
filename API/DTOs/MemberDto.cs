using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class MemberDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
    }
}
