using Back.Data;

using MediatR;

namespace Back.Common.Students.Commands.DeleteStudent
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, Unit>
    {
        private readonly ApplicationDbContext _context;
        public DeleteStudentCommandHandler(ApplicationDbContext context) => _context = context;

        public async Task<Unit> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _context.Students.FindAsync(new object[] { request.Id }, cancellationToken);

            if (student == null)
                throw new KeyNotFoundException($"Estudiante con ID {request.Id} no encontrado.");

            _context.Students.Remove(student);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
