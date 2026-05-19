using UniMeet.Shared.Abstractions;

namespace ModularSystem;

public interface IModuleRequestDispatcher
{
    Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default);
    Task SendAsync(IRequest request, CancellationToken cancellationToken = default);
}
