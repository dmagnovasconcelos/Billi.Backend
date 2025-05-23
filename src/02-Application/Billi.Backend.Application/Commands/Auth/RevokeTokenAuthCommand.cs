using Billi.Backend.Application.Commands.Auth.Responses;
using MediatR;

namespace Billi.Backend.Application.Commands.Auth
{
    public class RevokeTokenAuthCommand(string token) : IRequest<AuthResponse>
    {
        public string Token { get; } = token;
    }
}
