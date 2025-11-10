namespace UniMeet.Shared.Abstractions;

public interface IQueryHandler<TRequest, TResult> : IRequestHandler<TRequest, TResult>
    where TRequest : IQuery<TResult> { }