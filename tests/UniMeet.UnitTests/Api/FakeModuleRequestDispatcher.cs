using ModularSystem;
using UniMeet.Shared.Abstractions;

namespace UniMeet.UnitTests.Api;

internal sealed class FakeModuleRequestDispatcher : IModuleRequestDispatcher
{
    private readonly Queue<object?> _results = new();

    public List<object> SentRequests { get; } = new();

    public void QueueResult<T>(T result)
    {
        _results.Enqueue(result);
    }

    public Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default)
    {
        SentRequests.Add(request);

        if (_results.Count == 0)
        {
            throw new InvalidOperationException($"No result queued for {request.GetType().Name}.");
        }

        return Task.FromResult((TResult)_results.Dequeue()!);
    }

    public Task SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        SentRequests.Add(request);
        return Task.CompletedTask;
    }
}
