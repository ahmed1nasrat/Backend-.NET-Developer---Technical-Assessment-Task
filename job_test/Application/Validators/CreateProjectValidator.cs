using FluentValidation;
using job_test.Application.DTOs.Projects;

namespace job_test.Application.Validators
{
    public class CreateProjectValidator : AbstractValidator<CreateProjectDto>
    {
        public CreateProjectValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);

            RuleFor(x => x.Description) .NotEmpty().MaximumLength(500);
        }
    }
}
