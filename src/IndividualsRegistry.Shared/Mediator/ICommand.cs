using IndividualsRegistry.Shared.Library;
using MediatR;

namespace IndividualsRegistry.Shared.Mediator;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
