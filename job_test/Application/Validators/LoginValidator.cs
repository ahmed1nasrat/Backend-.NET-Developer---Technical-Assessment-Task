using FluentValidation;
using job_test.Application.DTOs.Auth;

namespace job_test.Application.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Password) .NotEmpty();
        }
    }
}
