namespace Back.Models
{
    public class Subject
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public int Credits { get; set; } = 3;
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;
        public ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
    }
}
