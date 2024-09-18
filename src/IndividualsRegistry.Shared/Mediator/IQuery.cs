using IndividualsRegistry.Shared.Library;
using MediatR;

namespace IndividualsRegistry.Shared.Mediator;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
