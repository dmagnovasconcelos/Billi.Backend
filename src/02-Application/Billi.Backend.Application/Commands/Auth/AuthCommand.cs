using Billi.Backend.Application.Commands.Auth.Responses;
using MediatR;

namespace Billi.Backend.Application.Commands.Auth
{
    public class AuthCommand : IRequest<AuthResponse>
    {
        public string Email { get; }

        public string Password { get; }
    }
}
