using IndividualsRegistry.Shared.Library;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IndividualsRegistry.Shared.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Starting request {@RequestName} {@Payload} at {@DateTime}",
            typeof(TRequest).Name,
            request,
            DateTime.Now
        );

        var result = await next();

        if (result.IsFailure)
        {
            _logger.LogError(
                "Request failure {@RequestName}, {@Error}, {@DateTime}",
                typeof(TRequest).Name,
                result.Errors.Select(error => error.Message),
                DateTime.Now
            );
        }

        _logger.LogInformation(
            "Completed request {@RequestName}, {@DateTime}",
            typeof(TRequest).Name,
            DateTime.Now
        );

        return result;
    }
}
