using Back.Data;
using Back.DTOs;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Back.Common.Students.Queries.GetClassmates
{
    public class GetClassmatesQueryHandler : IRequestHandler<GetClassmatesQuery, List<ClassmateDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetClassmatesQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClassmateDto>> Handle(GetClassmatesQuery request, CancellationToken cancellationToken)
        {
            // 1. Verificar si el estudiante existe
            var studentExists = await _context.Students
                .AnyAsync(s => s.Id == request.StudentId, cancellationToken);

            if (!studentExists)
            {
                throw new KeyNotFoundException($"El estudiante con ID {request.StudentId} no fue encontrado.");
            }

            // 2. Obtener las materias inscritas por el estudiante
            var studentSubjectIds = await _context.StudentSubjects
                .Where(ss => ss.StudentId == request.StudentId)
                .Select(ss => ss.SubjectId)
                .ToListAsync(cancellationToken);

            if (!studentSubjectIds.Any())
            {
                return new List<ClassmateDto>();
            }

            // 3. Obtener los nombres de los compañeros agrupados por materia
            var classmatesBySubject = await _context.StudentSubjects
                .Where(ss => studentSubjectIds.Contains(ss.SubjectId))
                .Include(ss => ss.Subject)
                .Include(ss => ss.Student)
                .GroupBy(ss => ss.Subject.Name)
                .Select(g => new ClassmateDto
                {
                    SubjectName = g.Key,
                    Classmates = g
                        .Where(ss => ss.StudentId != request.StudentId) // Excluir al propio estudiante
                        .Select(ss => ss.Student.Name)
                        .Distinct()
                        .ToList()
                })
                .ToListAsync(cancellationToken);

            return classmatesBySubject;
        }
    }
}
