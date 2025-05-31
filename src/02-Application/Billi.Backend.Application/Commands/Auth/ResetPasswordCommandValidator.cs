using FluentValidation;

namespace Billi.Backend.Application.Commands.Auth
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(150);
        }
    }
}
