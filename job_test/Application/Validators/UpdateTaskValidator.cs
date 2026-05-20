using FluentValidation;
using job_test.Application.DTOs.Tasks;

namespace job_test.Application.Validators
{
    public class UpdateTaskValidator : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskValidator()
        {
            RuleFor(x => x.Title).NotEmpty() .MaximumLength(100);

            RuleFor(x => x.Description) .NotEmpty().MaximumLength(500);
        }
    }
}
