using FluentValidation;
using job_test.Application.DTOs.Auth;

namespace job_test.Application.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(50);

            RuleFor(x => x.Email) .NotEmpty().EmailAddress();

            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }
}
