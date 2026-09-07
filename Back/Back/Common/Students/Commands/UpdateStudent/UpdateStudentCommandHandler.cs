using Back.Data;
using Back.Models;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Back.Common.Students.Commands.UpdateStudent
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, Unit>
    {
        private readonly ApplicationDbContext _context;
        public UpdateStudentCommandHandler(ApplicationDbContext context) => _context = context;

        public async Task<Unit> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _context.Students
                .Include(s => s.StudentSubjects)
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (student == null)
                throw new KeyNotFoundException($"Estudiante con ID {request.Id} no encontrado.");

            student.Name = request.Name;
            student.Email = request.Email;

            // Limpiar y reasignar materias
            _context.StudentSubjects.RemoveRange(student.StudentSubjects);
            foreach (var subjectId in request.SubjectIds)
            {
                student.StudentSubjects.Add(new StudentSubject { StudentId = student.Id, SubjectId = subjectId });
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
