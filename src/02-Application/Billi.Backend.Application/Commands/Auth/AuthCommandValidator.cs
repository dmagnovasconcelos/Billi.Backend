using FluentValidation;

namespace Billi.Backend.Application.Commands.Auth
{
    public class AuthCommandValidator : AbstractValidator<AuthCommand>
    {
        public AuthCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(150);

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .Length(8, 20);
        }
    }
}
