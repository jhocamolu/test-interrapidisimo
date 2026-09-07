using MediatR;

namespace Back.Common.Students.Commands.UpdateStudent
{
    public record UpdateStudentCommand(Guid Id, string Name, string Email, List<Guid> SubjectIds) : IRequest<Unit>;
}
