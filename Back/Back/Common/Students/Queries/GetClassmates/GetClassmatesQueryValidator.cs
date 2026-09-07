using FluentValidation;

namespace Back.Common.Students.Queries.GetClassmates
{
    public class GetClassmatesQueryValidator : AbstractValidator<GetClassmatesQuery>
    {
        public GetClassmatesQueryValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("El ID del estudiante es obligatorio.");
        }
    }
}
