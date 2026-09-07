using Back.DTOs;

using MediatR;

namespace Back.Common.Students.Queries.GetClassmates
{
    public record GetClassmatesQuery(Guid StudentId) : IRequest<List<ClassmateDto>>;
}
