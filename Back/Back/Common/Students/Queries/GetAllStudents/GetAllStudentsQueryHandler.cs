using Back.Data;
using Back.DTOs;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Back.Common.Students.Queries.GetAllStudents
{
    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, List<StudentDetailDto>>
    {
        private readonly ApplicationDbContext _context;
        public GetAllStudentsQueryHandler(ApplicationDbContext context) => _context = context;

        public async Task<List<StudentDetailDto>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Students
                .Select(s => new StudentDetailDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    Subjects = s.StudentSubjects.Select(ss => ss.Subject.Name).ToList()
                })
                .ToListAsync(cancellationToken);
        }
    }
}
