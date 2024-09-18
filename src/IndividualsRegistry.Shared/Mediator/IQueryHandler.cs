using IndividualsRegistry.Shared.Library;
using MediatR;

namespace IndividualsRegistry.Shared.Mediator;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
