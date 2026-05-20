using FluentValidation;
using job_test.Application.DTOs.Tasks;

namespace job_test.Application.Validators
{
    public class CreateTaskValidator : AbstractValidator<CreateTaskDto>
    {
        public CreateTaskValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);

            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);

            RuleFor(x => x.ProjectId).GreaterThan(0);
        }
    }
}
