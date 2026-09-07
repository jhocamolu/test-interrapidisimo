using Back.Data;
using Back.Models;

using MediatR;

namespace Back.Common.Students.Commands.RegisterStudent
{
    public class RegisterStudentCommandHandler : IRequestHandler<RegisterStudentCommand, Guid>
    {
        private readonly ApplicationDbContext _context;

        public RegisterStudentCommandHandler(ApplicationDbContext context) => _context = context;

        public async Task<Guid> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
        {
            var student = new Student { Name = request.Name, Email = request.Email };

            foreach (var subjectId in request.SubjectIds)
            {
                student.StudentSubjects.Add(new StudentSubject { StudentId = student.Id, SubjectId = subjectId });
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync(cancellationToken);
            return student.Id;
        }
    }
}
