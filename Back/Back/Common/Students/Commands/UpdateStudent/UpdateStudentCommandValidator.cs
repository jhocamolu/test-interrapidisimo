using Back.Data;

using Microsoft.EntityFrameworkCore;

using FluentValidation;

namespace Back.Common.Students.Commands.UpdateStudent
{
    public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
    {
        private readonly ApplicationDbContext _context;

        public UpdateStudentCommandValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es obligatorio.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Correo inválido.");
            RuleFor(x => x.SubjectIds)
                .Must(ids => ids != null && ids.Count == 3)
                .WithMessage("Debe seleccionar exactamente 3 materias.");

            RuleFor(x => x.SubjectIds)
                .MustAsync(async (subjectIds, cancellation) =>
                {
                    if (subjectIds == null || subjectIds.Count != 3) return false;
                    var teacherIds = await _context.Subjects
                        .Where(s => subjectIds.Contains(s.Id))
                        .Select(s => s.TeacherId)
                        .ToListAsync(cancellation);

                    return teacherIds.Distinct().Count() == subjectIds.Count;
                })
                .WithMessage("No puede seleccionar materias con el mismo profesor.");
        }
    }
}
