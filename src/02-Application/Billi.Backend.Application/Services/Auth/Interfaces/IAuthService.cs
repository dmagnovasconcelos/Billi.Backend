using Billi.Backend.Application.Commands.Auth;
using Billi.Backend.CrossCutting.Responses;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Billi.Backend.Application.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<Response> Login(string authHeader, CancellationToken cancellationToken);
        Task<Response> Logout(CancellationToken cancellationToken);
        Task<Response> RefreshToken(RefreshTokenAuthCommand command, CancellationToken cancellationToken);
        Task<Response> ResetPassword(ResetPasswordCommand command, CancellationToken cancellationToken);
        Task<Response> ResetPasswordConfirm(ConfirmResetPasswordCommand command, CancellationToken cancellationToken);
        Task<Response> ValidateResetPasswordCode(ValidateResetPasswordCodeCommand command, CancellationToken cancellationToken);
    }
}
