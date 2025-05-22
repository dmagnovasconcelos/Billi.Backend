using Billi.Backend.Application.Commands.Auth.Responses;
using Billi.Backend.Application.Services;
using Billi.Backend.CrossCutting.Enums;
using Billi.Backend.CrossCutting.Utilities;
using Billi.Backend.Domain.Interfaces.Users;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Billi.Backend.Application.Commands.Auth
{
    public class AuthCommandHandler(IConfiguration configuration,
        ILogger<AuthCommandHandler> logger,
        IUserQueryRepository repository, 
        IAccessTokenGeneratorService service) : IRequestHandler<AuthCommand, AuthResponse>
    {
        public async Task<AuthResponse> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await repository.GetAsync(x => x.Email.Equals(request.Email), cancellationToken);
                if (user is null)
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.NotFound);

                if (user.Status != StatusType.Active)
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.InactiveUser);

                if (!user.Password.Equals(request.Password.Encrypt(configuration)))
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.InvalidPassword);

                var accessToken = service.GenerateAccessToken(user.Email);
                return AuthResponse.SuccessResult("Authorized", ResponseSuccessType.Accepted, accessToken) as AuthResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing authentication request: {Message}", ex.Message);
                return AuthResponse.Error(ex.Message) as AuthResponse;
            }
        }
    }
}
