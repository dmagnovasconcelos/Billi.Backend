using Billi.Backend.Application.Commands.Auth.Responses;
using Billi.Backend.Application.Services.Auth.Interfaces;
using Billi.Backend.CrossCutting.Enums;
using Billi.Backend.CrossCutting.Responses;
using Billi.Backend.Domain.Interfaces.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Billi.Backend.Application.Commands.Auth
{
    public class RevokeTokenAuthCommandHandler(ILogger<RevokeTokenAuthCommandHandler> logger,
        IUserUnitOfWork unitOfWork,
        ITokenBlacklistService tokenBlacklistService,
        IValidator<RevokeTokenAuthCommand> validator) : IRequestHandler<RevokeTokenAuthCommand, Response>
    {
        public async Task<Response> Handle(RevokeTokenAuthCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var validatorResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validatorResult.IsValid)
                    return Response.InvalidCommand(validatorResult.Errors);

                await unitOfWork.Start();
                var user = await unitOfWork.UserRepository.GetAsync(x => x.Id.Equals(request.UserId) && x.Email.ToUpper().Equals(request.Email.ToUpper()), x => x.Include(y => y.UserRefreshToken), cancellationToken);
                if (user is null)
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.NotFound);

                if (user.Status != StatusType.Active)
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.InactiveUser);

                await tokenBlacklistService.AddTokenAsync(request.Token);

                user.UserRefreshToken.IsRevoked = true;
                unitOfWork.RefreshTokenRepository.Update(user.UserRefreshToken);

                await unitOfWork.CommitAsync();

                return Response.SuccessResult("Revoked Token", ResponseSuccessType.Accepted);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing authentication revoke request: {Message}", ex.Message);

                await unitOfWork.RollbackAsync();
                return Response.Error(ex.Message);
            }
        }
    }
}
