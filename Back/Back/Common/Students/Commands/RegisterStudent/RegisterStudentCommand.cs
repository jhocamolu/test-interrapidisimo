using MediatR;

namespace Back.Common.Students.Commands.RegisterStudent
{
    public record RegisterStudentCommand(string Name, string Email, List<Guid> SubjectIds) : IRequest<Guid>;
   
}
