using FluentValidation;

namespace Billi.Backend.Application.Commands.Auth
{
    public class RefreshTokenAuthCommandValidator : AbstractValidator<RefreshTokenAuthCommand>
    {
        public RefreshTokenAuthCommandValidator()
        {
            RuleFor(x => x.Token)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.RefreshToken)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
