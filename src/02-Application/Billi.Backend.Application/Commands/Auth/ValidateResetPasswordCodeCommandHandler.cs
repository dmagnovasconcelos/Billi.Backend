using Billi.Backend.CrossCutting.Enums;
using Billi.Backend.CrossCutting.Responses;
using Billi.Backend.Domain.Interfaces.Users;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Billi.Backend.Application.Commands.Auth
{
    public class ValidateResetPasswordCodeCommandHandler(ILogger<ValidateResetPasswordCodeCommandHandler> logger,
        IUserQueryRepository queryRepository,
        IValidator<ValidateResetPasswordCodeCommand> validator) : IRequestHandler<ValidateResetPasswordCodeCommand, Response>
    {
        public async Task<Response> Handle(ValidateResetPasswordCodeCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var validatorResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validatorResult.IsValid)
                    return Response.InvalidCommand(validatorResult.Errors);


                var validCode = await queryRepository.ExistsAsync(x => x.Email.ToUpper().Equals(request.Email.ToUpper()) &&
                                                x.ValidationResetPassword.Equals(request.Code) &&
                                                x.RedefinePassword &&
                                                x.Status == StatusType.Active, cancellationToken: cancellationToken);

                if (validCode)
                    return Response.SuccessResult("Code is valid", ResponseSuccessType.Accepted);

                return Response.UnsuccessfulResult("Invalid code or user not found");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing validate reset password request: {Message}", ex.Message);

                return Response.Error(ex.Message);
            }
        }
    }
}
