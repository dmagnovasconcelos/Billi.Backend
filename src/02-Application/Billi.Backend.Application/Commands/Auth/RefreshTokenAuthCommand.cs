using Billi.Backend.Application.Commands.Auth.Responses;
using Billi.Backend.CrossCutting.Responses;
using MediatR;

namespace Billi.Backend.Application.Commands.Auth
{
    public class RefreshTokenAuthCommand(string token, string refreshToken) : IRequest<Response>
    {
        public string Token { get; } = token;

        public string RefreshToken { get; } = refreshToken;
    }
}