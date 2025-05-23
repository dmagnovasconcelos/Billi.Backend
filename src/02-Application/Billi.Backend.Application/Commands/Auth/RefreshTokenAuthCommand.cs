using Billi.Backend.Application.Commands.Auth.Responses;
using MediatR;

namespace Billi.Backend.Application.Commands.Auth
{
    public class RefreshTokenAuthCommand : IRequest<AuthResponse>
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
