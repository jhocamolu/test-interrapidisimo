using Back.DTOs;
using MediatR;

namespace Back.Common.Students.Queries.GetAllStudents
{
    public record GetAllStudentsQuery() : IRequest<List<StudentDetailDto>>;
}
