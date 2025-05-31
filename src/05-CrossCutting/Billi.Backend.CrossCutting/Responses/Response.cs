using Billi.Backend.CrossCutting.Enums;
using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace Billi.Backend.CrossCutting.Responses
{
    public class Response
    {
        public Response(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public Response(bool success, string message, ResponseSuccessType responseSuccess)
        {
            Success = success;
            Message = message;
            ResponseSuccess = responseSuccess;
        }

        public Response(bool success, string message, ResponseFailureType responseFailure)
        {
            Success = success;
            Message = message;
            ResponseFailure = responseFailure;
        }

        public object Data { get; set; }

        public string Message { get; init; }

        [JsonIgnore]
        public ResponseFailureType ResponseFailure { get; }

        [JsonIgnore]
        public ResponseSuccessType ResponseSuccess { get; }

        public bool Success { get; }

        public static Response SuccessResult(string message = null, ResponseSuccessType typeOfResponseSuccess = ResponseSuccessType.Success, object data = null)
        {
            return new(true, message, typeOfResponseSuccess)
            {
                Data = data
            };
        }

        public static Response UnsuccessfulResult(string message)
        {
            return new(false, message, ResponseFailureType.Unsuccesss);
        }

        public static Response Error(string message)
        {
            return new(false, message, ResponseFailureType.Error);
        }

        public static Response InvalidCommand(string message)
        {
            return new(false, message, ResponseFailureType.InvalidCommand);
        }

        public static Response InvalidCommand(List<ValidationFailure> errors)
        {
            return new(false, "Invalid Command", ResponseFailureType.InvalidCommand)
            {
                Data = errors
            };
        }
    }
}