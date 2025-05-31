using AutoMapper;
using Billi.Backend.Application.Commands.Auth.Responses;
using Billi.Backend.Application.Services.Auth.Interfaces;
using Billi.Backend.CrossCutting.Enums;
using Billi.Backend.CrossCutting.Responses;
using Billi.Backend.CrossCutting.Utilities;
using Billi.Backend.Domain.Interfaces.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Billi.Backend.Application.Commands.Auth
{
    public class RefreshTokenAuthCommandHandler(
        ILogger<RefreshTokenAuthCommandHandler> logger,
        IMapper mapper,
        IUserUnitOfWork unitOfWork,
        IAccessTokenGeneratorService service,
        IValidator<RefreshTokenAuthCommand> validator) : IRequestHandler<RefreshTokenAuthCommand, Response>
    {
        public async Task<Response> Handle(RefreshTokenAuthCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var validatorResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validatorResult.IsValid)
                    return Response.InvalidCommand(validatorResult.Errors);

                var session = await service.ValidateToken(request.Token, cancellationToken);

                await unitOfWork.Start();
                var user = await unitOfWork.UserRepository.GetAsync(x => x.Id.Equals(session.UserId) && x.Email.ToUpper().Equals(session.Email.ToUpper()), x => x.Include(y => y.UserRefreshToken), cancellationToken);
                if (user is null)
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.NotFound);

                if (user.Status != StatusType.Active)
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.InactiveUser);

                if (user.UserRefreshToken is null || user.UserRefreshToken.Token != request.RefreshToken)
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.InvalidRefreshToken);

                if (user.UserRefreshToken.IsRevoked)
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.RevokedRefreshToken);

                if (user.UserRefreshToken.ExpiresAt.IsExpired())
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.ExpiredRefreshToken);

                var accessToken = service.GenerateAccessToken(user.Email, user.Id, cancellationToken);

                mapper.Map(accessToken, user.UserRefreshToken);
                user.UserRefreshToken.IsUsed = true;

                unitOfWork.RefreshTokenRepository.Update(user.UserRefreshToken);
                await unitOfWork.CommitAsync();

                return Response.SuccessResult("Authorized", ResponseSuccessType.Accepted, accessToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing authentication refresh request: {Message}", ex.Message);
                await unitOfWork.RollbackAsync();
                return Response.Error(ex.Message);
            }
        }
    }
}
