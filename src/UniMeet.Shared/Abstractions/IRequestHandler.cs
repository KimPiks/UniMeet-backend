namespace UniMeet.Shared.Abstractions;

public interface IRequestHandler<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    Task<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IRequestHandler<TRequest>
    where TRequest : IRequest
{
    Task HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}