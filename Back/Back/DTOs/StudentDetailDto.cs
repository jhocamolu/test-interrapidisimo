namespace Back.DTOs
{
    public class StudentDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Subjects { get; set; } = new();
    }
}
