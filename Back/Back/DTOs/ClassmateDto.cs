namespace Back.DTOs;

public class ClassmateDto
{
    public string SubjectName { get; set; } = string.Empty;
    public List<string> Classmates { get; set; } = new();
}