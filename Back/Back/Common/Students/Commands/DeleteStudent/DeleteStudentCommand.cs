using MediatR;

namespace Back.Common.Students.Commands.DeleteStudent
{
    public record DeleteStudentCommand(Guid Id) : IRequest<Unit>;
}
