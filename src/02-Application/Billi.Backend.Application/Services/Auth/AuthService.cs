using Billi.Backend.Application.Commands.Auth;
using Billi.Backend.Application.Services.Auth.Interfaces;
using Billi.Backend.CrossCutting.Auth;
using Billi.Backend.CrossCutting.Responses;
using MediatR;
using System.Text;

namespace Billi.Backend.Application.Services.Auth
{
    public class AuthService(ICurrentUser currentUser, IMediator mediator) : IAuthService
    {
        public async Task<Response> Login(string authHeader, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Basic "))
            {
                return Response.InvalidCommand("Missing or invalid Authorization header");
            }

            var encodedCredentials = authHeader["Basic ".Length..].Trim();
            var decodedBytes = Convert.FromBase64String(encodedCredentials);
            var decodedCredentials = Encoding.UTF8.GetString(decodedBytes);

            var parts = decodedCredentials.Split(':', 2);
            if (parts.Length != 2)
            {
                return Response.Error("Invalid credentials format");
            }

            var email = parts[0];
            var password = parts[1];

            var result = await mediator.Send(new AuthCommand(email, password), cancellationToken);
            return result;
        }

        public async Task<Response> Logout(CancellationToken cancellationToken) 
            => await mediator.Send(new RevokeTokenAuthCommand(currentUser.UserId.Value, currentUser.Email, currentUser.Token), cancellationToken);

        public Task<Response> RefreshToken(RefreshTokenAuthCommand command, CancellationToken cancellationToken) 
            => mediator.Send(command, cancellationToken);

        public Task<Response> ResetPassword(ResetPasswordCommand command, CancellationToken cancellationToken)
            => mediator.Send(command, cancellationToken);

        public Task<Response> ResetPasswordConfirm(ConfirmResetPasswordCommand command, CancellationToken cancellationToken)
            => mediator.Send(command, cancellationToken);

        public Task<Response> ValidateResetPasswordCode(ValidateResetPasswordCodeCommand command, CancellationToken cancellationToken)
            => mediator.Send(command, cancellationToken);
    }
}
