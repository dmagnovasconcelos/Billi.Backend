using FluentValidation;

namespace Billi.Backend.Application.Commands.Auth
{
    public class ConfirmResetPasswordCommandValidator : AbstractValidator<ConfirmResetPasswordCommand>
    {
        public ConfirmResetPasswordCommandValidator()
        {
            Include(new ValidateResetPasswordCodeCommandValidator());

            RuleFor(x => x.NewPassword)
                .NotNull()
                .NotEmpty()
                .Length(8, 20);
        }
    }
}
