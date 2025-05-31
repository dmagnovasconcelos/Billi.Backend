using Billi.Backend.CrossCutting.Responses;
using MediatR;

namespace Billi.Backend.Application.Commands.Auth
{
    public class RevokeTokenAuthCommand(Guid userId, string email, string token) : IRequest<Response>
    {
        public Guid UserId { get; } = userId;
        public string Email { get; } = email;
        public string Token { get; } = token;
    }
}