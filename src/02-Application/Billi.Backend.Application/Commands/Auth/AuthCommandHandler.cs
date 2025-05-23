using AutoMapper;
using Billi.Backend.Application.Commands.Auth.Responses;
using Billi.Backend.Application.Services;
using Billi.Backend.CrossCutting.Enums;
using Billi.Backend.CrossCutting.Responses;
using Billi.Backend.CrossCutting.Utilities;
using Billi.Backend.Domain.Entities;
using Billi.Backend.Domain.Interfaces.Users;
using Billi.Backend.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Billi.Backend.Application.Commands.Auth
{
    public class AuthCommandHandler(IConfiguration configuration,
        ILogger<AuthCommandHandler> logger,
        IMapper mapper,
        IUserUnitOfWork unitOfWork,
        IAccessTokenGeneratorService service) :
        IRequestHandler<AuthCommand, AuthResponse>,
        IRequestHandler<RefreshTokenAuthCommand, AuthResponse>,
        IRequestHandler<RevokeTokenAuthCommand, AuthResponse>
    {
        public async Task<AuthResponse> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await unitOfWork.Start();
                var user = await unitOfWork.Repository.GetAsync(x => x.Email.Equals(request.Email, StringComparison.InvariantCultureIgnoreCase), x => x.Include(y => y.UserRefreshToken), cancellationToken);
                if (user is null)
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.NotFound);

                if (user.Status != StatusType.Active)
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.InactiveUser);

                if (!user.Password.Equals(request.Password.Encrypt(configuration)))
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.InvalidPassword);

                var accessToken = service.GenerateAccessToken(user.Email, user.Id, cancellationToken);

                if (user.UserRefreshToken is not null)
                {
                    mapper.Map(accessToken, user.UserRefreshToken);

                    unitOfWork.RefreshTokenRepository.Update(user.UserRefreshToken);
                }
                else
                {
                    var refreshToken = mapper.Map<UserRefreshTokenEntity>(accessToken);

                    await unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
                }

                await unitOfWork.CommitAsync();

                return Response.SuccessResult("Authorized", ResponseSuccessType.Accepted, accessToken) as AuthResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing authentication request: {Message}", ex.Message);

                await unitOfWork.RollbackAsync();
                return Response.Error(ex.Message) as AuthResponse;
            }
        }

        public async Task<AuthResponse> Handle(RefreshTokenAuthCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var session = await service.ValidateToken(request.Token, cancellationToken);

                await unitOfWork.Start();
                var user = await unitOfWork.Repository.GetAsync(x => x.Id.Equals(session.UserId) && x.Email.Equals(session.Email, StringComparison.InvariantCultureIgnoreCase), x => x.Include(y => y.UserRefreshToken), cancellationToken);
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

                return Response.SuccessResult("Authorized", ResponseSuccessType.Accepted, accessToken) as AuthResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing authentication refresh request: {Message}", ex.Message);
                return Response.Error(ex.Message) as AuthResponse;
            }
        }

        public async Task<AuthResponse> Handle(RevokeTokenAuthCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var session = new SessionAuth(request.Token);

                await unitOfWork.Start();
                var user = await unitOfWork.Repository.GetAsync(x => x.Id.Equals(session.UserId) && x.Email.Equals(session.Email, StringComparison.InvariantCultureIgnoreCase), x => x.Include(y => y.UserRefreshToken).Include(y => y.UserRevokedTokens), cancellationToken);
                if (user is null)
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.NotFound);

                if (user.Status != StatusType.Active)
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.InactiveUser);

                var revokedToken = mapper.Map<UserRevokedTokenEntity>((user.Id, request));

                await unitOfWork.RevokedTokenRepository.AddAsync(revokedToken);

                user.UserRefreshToken.IsRevoked = true;
                unitOfWork.RefreshTokenRepository.Update(user.UserRefreshToken);

                await unitOfWork.CommitAsync();

                return Response.SuccessResult("Revoked Token", ResponseSuccessType.Accepted) as AuthResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing authentication revoke request: {Message}", ex.Message);
                return Response.Error(ex.Message) as AuthResponse;
            }
        }
    }
}
