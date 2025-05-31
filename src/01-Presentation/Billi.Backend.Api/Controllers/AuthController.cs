using Asp.Versioning;
using Billi.Backend.Application.Commands.Auth;
using Billi.Backend.Application.Services.Auth.Interfaces;
using Billi.Backend.CrossCutting.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Billi.Backend.Infra.IoC.DependencyInjection;

namespace Billi.Backend.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class AuthController(IAuthService service) : ApiController
    {
        [HttpPost("login")]
        [BasicAuth]
        [AllowAnonymous]
        public async Task<IActionResult> Login(CancellationToken cancellationToken)
        {
            var result = await service.Login(Request.Headers.Authorization.ToString(), cancellationToken);

            return CustomResponse(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            var result = await service.Logout(cancellationToken);

            return CustomResponse(result);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenAuthCommand command, CancellationToken cancellationToken)
        {
            var result = await service.RefreshToken(command, cancellationToken);

            return CustomResponse(result);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var result = await service.ResetPassword(command, cancellationToken);

            return CustomResponse(result);
        }

        [HttpPost("reset-password/validate-code")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateResetPasswordCode([FromBody] ValidateResetPasswordCodeCommand command, CancellationToken cancellationToken)
        {
            var result = await service.ValidateResetPasswordCode(command, cancellationToken);

            return CustomResponse(result);
        }

        [HttpPost("reset-password/confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordConfirm([FromBody] ConfirmResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var result = await service.ResetPasswordConfirm(command, cancellationToken);

            return CustomResponse(result);
        }
    }
}