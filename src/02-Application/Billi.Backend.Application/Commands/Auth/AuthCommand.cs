using Billi.Backend.Application.Commands.Auth.Responses;
using MediatR;

namespace Billi.Backend.Application.Commands.Auth
{
    public class AuthCommand(string email, string password) : IRequest<AuthResponse>
    {
        public string Email { get; } = email;

        public string Password { get; } = password;
    }
}
