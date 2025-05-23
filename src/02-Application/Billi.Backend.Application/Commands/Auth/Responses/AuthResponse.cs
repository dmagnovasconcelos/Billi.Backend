using Billi.Backend.CrossCutting.Enums;
using Billi.Backend.CrossCutting.Responses;
using Billi.Backend.CrossCutting.Utilities;

namespace Billi.Backend.Application.Commands.Auth.Responses
{
    public class AuthResponse : Response
    {
        public AuthResponse(bool success, string message, ResponseSuccessType responseSuccess)
            : base(success, message, responseSuccess) { }
        public AuthResponse(bool success, string message, ResponseFailureType responseFailure)
            : base(success, message, responseFailure) { }

        public static AuthResponse Unauthorized(ResponseUnauthorizedType responseUnauthorized)
        {
            return new(false, responseUnauthorized.GetDescription().Description, ResponseFailureType.NotAuthorized);
        }

    }
}
