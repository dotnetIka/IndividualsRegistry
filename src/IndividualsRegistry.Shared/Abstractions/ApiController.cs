using IndividualsRegistry.Shared.Library;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IndividualsRegistry.Shared.Abstractions;

[ApiController]
public abstract class ApiController : ControllerBase
{
    protected readonly ISender Sender;

    protected ApiController(ISender sender) => Sender = sender;

    //protected async Task<IActionResult> HandleCommand<TCommand>(TCommand command, CancellationToken cancellationToken)
    //    where TCommand : IRequest<Result>
    //{
    //    var validator = HttpContext.RequestServices.GetService<IValidator<TCommand>>();
    //    if (validator != null)
    //    {
    //        var validationResult = await validator.ValidateAsync(command, cancellationToken);
    //        if (!validationResult.IsValid)
    //        {
    //            var errors = validationResult.Errors.Select(e => new Error(e.PropertyName, e.ErrorMessage, ErrorTypeEnum.BadRequest)).ToArray();
    //            return BadRequest(Result.Failure(errors));
    //        }
    //    }

    //    var result = await Sender.Send(command, cancellationToken);
    //    return HandleResult(result);
    //}

    //protected async Task<IActionResult> HandleQuery<TQuery>(TQuery query, CancellationToken cancellationToken)
    //    where TQuery : IRequest<Result>
    //{
    //    var validator = HttpContext.RequestServices.GetService<IValidator<TQuery>>();
    //    if (validator != null)
    //    {
    //        var validationResult = await validator.ValidateAsync(query, cancellationToken);
    //        if (!validationResult.IsValid)
    //        {
    //            var errors = validationResult.Errors.Select(e => new Error(e.PropertyName, e.ErrorMessage, ErrorTypeEnum.BadRequest)).ToArray();
    //            return BadRequest(Result.Failure(errors));
    //        }
    //    }

    //    var result = await Sender.Send(query, cancellationToken);
    //    return HandleResult(result);
    //}

    protected IActionResult HandleResult(Result result)
    {
        return result switch
        {
            { IsSuccess: true } => Ok(result),
            { IsSuccess: false, Errors: var errors } when errors.Length > 0 =>
                errors[0].Code switch
                {
                    nameof(HttpStatusCode.NotFound) => NotFound(result),
                    nameof(HttpStatusCode.BadRequest) => BadRequest(result),
                    nameof(HttpStatusCode.Unauthorized) => Unauthorized(result),
                    nameof(HttpStatusCode.Forbidden) => Forbid(),
                    nameof(HttpStatusCode.Conflict) => Conflict(result),
                    nameof(HttpStatusCode.UnprocessableEntity) => UnprocessableEntity(result),
                    _ => BadRequest(result)
                },
            _ => BadRequest(new { error = "An unexpected error occurred." })
        };
    }
}
