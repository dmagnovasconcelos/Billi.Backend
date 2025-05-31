using AutoMapper;
using Billi.Backend.Application.Commands.Auth.Responses;
using Billi.Backend.Application.Services.Auth.Interfaces;
using Billi.Backend.CrossCutting.Enums;
using Billi.Backend.CrossCutting.Responses;
using Billi.Backend.Domain.Entities;
using Billi.Backend.Domain.Interfaces.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Billi.Backend.Application.Commands.Auth
{
    public class AuthCommandHandler(
        ILogger<AuthCommandHandler> logger,
        IMapper mapper,
        IUserUnitOfWork unitOfWork,
        IAccessTokenGeneratorService service,
        IValidator<AuthCommand> validator) : IRequestHandler<AuthCommand, Response>
    {
        public async Task<Response> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var validatorResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validatorResult.IsValid)
                    return Response.InvalidCommand(validatorResult.Errors);

                await unitOfWork.Start();
                var user = await unitOfWork.UserRepository.GetAsync(x => x.Email.ToUpper().Equals(request.Email.ToUpper()), x => x.Include(y => y.UserRefreshToken), cancellationToken);
                if (user is null)
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.NotFound);

                if (user.Status != StatusType.Active)
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.InactiveUser);

                if (!user.Password.Equals(request.Password))
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

                user.ValidationResetPassword = null;
                user.RedefinePassword = false;

                unitOfWork.UserRepository.Update(user);

                await unitOfWork.CommitAsync();

                return Response.SuccessResult("Authorized", ResponseSuccessType.Accepted, accessToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing authentication request: {Message}", ex.Message);

                await unitOfWork.RollbackAsync();
                return Response.Error(ex.Message);
            }
        }
    }
}