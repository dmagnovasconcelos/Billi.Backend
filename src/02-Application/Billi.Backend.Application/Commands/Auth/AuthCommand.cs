using Billi.Backend.CrossCutting.Responses;
using MediatR;

namespace Billi.Backend.Application.Commands.Auth
{
    public class AuthCommand(string email, string password) : IRequest<Response>
    {
        public string Email { get; } = email;

        public string Password { get; } = password;
    }
}