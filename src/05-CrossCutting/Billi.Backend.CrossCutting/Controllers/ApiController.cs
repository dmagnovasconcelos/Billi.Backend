using Billi.Backend.CrossCutting.Enums;
using Billi.Backend.CrossCutting.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace Billi.Backend.CrossCutting.Controllers
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected IActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors).Select(x => x.ErrorMessage);

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Messages", errors.ToArray() }
            }));
        }

        protected IActionResult CustomResponse(Response response)
        {
            if (!response.Success)
            {
                return response.ResponseFailure switch
                {
                    ResponseFailureType.Error => StatusCode((int)HttpStatusCode.InternalServerError, response),
                    ResponseFailureType.NotAuthorized => Unauthorized(response),
                    ResponseFailureType.NotFound => NotFound(),
                    _ => BadRequest(response)
                };
            }

            return response.ResponseSuccess switch
            {
                ResponseSuccessType.Created => Created(string.Empty, response),
                ResponseSuccessType.Accepted => Accepted(response),
                ResponseSuccessType.NoContent => NoContent(),
                _ => Ok(response)
            };
        }
    }
}
