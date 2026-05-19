using UniMeet.Shared.Abstractions;

namespace ModularSystem;

internal sealed class ModuleRequestDispatcher(IMediator mediator) : IModuleRequestDispatcher
{
    public Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default)
        => mediator.SendAsync(request, cancellationToken);

    public Task SendAsync(IRequest request, CancellationToken cancellationToken = default)
        => mediator.SendAsync(request, cancellationToken);
}
