using Billi.Backend.Application.Commands.Auth.Responses;
using Billi.Backend.Application.Services.Email.Interfaces;
using Billi.Backend.CrossCutting.Enums;
using Billi.Backend.CrossCutting.Responses;
using Billi.Backend.CrossCutting.Utilities;
using Billi.Backend.Domain.Interfaces.Users;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Billi.Backend.Application.Commands.Auth
{
    public class ResetPasswordCommandHandler(ILogger<ResetPasswordCommandHandler> logger,
        IEmailService emailService,
        IUserUnitOfWork unitOfWork,
        IValidator<ResetPasswordCommand> validator) : IRequestHandler<ResetPasswordCommand, Response>
    {
        public async Task<Response> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var validatorResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validatorResult.IsValid)
                    return Response.InvalidCommand(validatorResult.Errors);

                await unitOfWork.Start();
                var user = await unitOfWork.UserRepository.GetAsync(x => x.Email.ToUpper().Equals(request.Email.ToUpper()), cancellationToken: cancellationToken);
                if (user is null)
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.NotFound);

                if (user.Status != StatusType.Active)
                    return AuthResponse.Unauthorized(ResponseUnauthorizedType.InactiveUser);

                user.ValidationResetPassword = Utility.GenerateAlphaNumericCode();
                user.RedefinePassword = true;

                unitOfWork.UserRepository.Update(user);

                var htmlBody = EmailTemplates.GenerateResetPasswordEmail(user.ValidationResetPassword);
                
                await emailService.SendAsync(user.Email, "Redefinição de Senha", htmlBody, cancellationToken);

                await unitOfWork.CommitAsync();

                return Response.SuccessResult("Password reset code sent", ResponseSuccessType.Accepted);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing reset password request: {Message}", ex.Message);

                await unitOfWork.RollbackAsync();
                return Response.Error(ex.Message);
            }
        }
    }   
}
