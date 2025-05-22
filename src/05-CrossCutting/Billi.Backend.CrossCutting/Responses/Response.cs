using Billi.Backend.CrossCutting.Enums;
using System.Text.Json.Serialization;

namespace Billi.Backend.CrossCutting.Responses
{
    public class Response
    {
        public Response(bool success
            , string message
            , ResponseSuccessType responseSuccess)
        {
            Success = success;
            Message = message;
            ResponseSuccess = responseSuccess;
        }

        public Response(bool success
            , string message
            , ResponseFailureType responseFailure)
        {
            Success = success;
            Message = message;
            ResponseFailure = responseFailure;
        }

        public object Data { get; init; }

        public string Message { get; }

        [JsonIgnore]
        public ResponseFailureType ResponseFailure { get; }

        [JsonIgnore]
        public ResponseSuccessType ResponseSuccess { get; }

        public bool Success { get; }

        public static Response SuccessResult(string message, ResponseSuccessType typeOfResponseSuccess = ResponseSuccessType.Ok, object data = null)
        {
            return new(true, message, typeOfResponseSuccess)
            {
                Data = data
            };
        }

        public static Response Error(string message)
        {
            return new(false, message, ResponseFailureType.Error);
        }

        public static Response InvalidCommand(string message)
        {
            return new(false, message, ResponseFailureType.InvalidCommand);
        }
    }
}