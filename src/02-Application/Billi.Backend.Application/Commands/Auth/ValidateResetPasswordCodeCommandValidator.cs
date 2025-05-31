using FluentValidation;

namespace Billi.Backend.Application.Commands.Auth
{
    public class ValidateResetPasswordCodeCommandValidator : AbstractValidator<ValidateResetPasswordCodeCommand>
    {
        public ValidateResetPasswordCodeCommandValidator()
        {
            Include(new ResetPasswordCommandValidator());

            RuleFor(x => x.Code)
                .NotNull()
                .NotEmpty()
                .Length(10);
        }
    }
}
