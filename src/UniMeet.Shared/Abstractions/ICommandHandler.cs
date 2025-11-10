namespace UniMeet.Shared.Abstractions;

public interface ICommandHandler<TRequest> : IRequestHandler<TRequest>
    where TRequest : ICommand { }