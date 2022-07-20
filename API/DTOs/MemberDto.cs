namespace API.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
