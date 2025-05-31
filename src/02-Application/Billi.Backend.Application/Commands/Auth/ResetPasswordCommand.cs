using Billi.Backend.CrossCutting.Responses;
using MediatR;

namespace Billi.Backend.Application.Commands.Auth
{
    public class ResetPasswordCommand(string email) : IRequest<Response>
    {
        public string Email { get; } = email;
    }
}
