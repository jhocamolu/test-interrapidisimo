namespace Back.Models
{
    public class Teacher
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
