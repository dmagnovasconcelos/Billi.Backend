using FluentValidation;

namespace Billi.Backend.Application.Commands.Auth
{
    public class RevokeTokenAuthCommandValidator : AbstractValidator<RevokeTokenAuthCommand>
    {
        public RevokeTokenAuthCommandValidator()
        {
            RuleFor(x => x.Token)
                .NotNull()
                .NotEmpty();
        }
    }
}
