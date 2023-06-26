using AtivaBank.Domain.Response;
using Microsoft.AspNetCore.Mvc;

namespace AtivaBank.Controllers.Base
{
    public class DefaultController : ControllerBase
    {
        public IActionResult GetResponse<TResponse>(Response<TResponse> source)
           => source.IsSuccess is false ? ValidateError(source) : Ok(source.Data);

        public IActionResult GetResponseCreated<TResponse>(Response<TResponse> source)
            => source.IsSuccess is false ? ValidateError(source) : Created(Route ?? "/", source.Data);

        public IActionResult GetResponseNoContent<TResponse>(Response<TResponse> source)
            => source.IsSuccess is false ? ValidateError(source) : NoContent();

        private IActionResult ValidateError<TResponse>(Response<TResponse> source)
            => BadRequest(source);

        private string Route => HttpContext?.Request?.Path.Value;
    }
}
