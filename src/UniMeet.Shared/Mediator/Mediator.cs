using UniMeet.Shared.Abstractions;

namespace UniMeet.Shared.Mediator;

public class Mediator(ServiceFactory serviceFactory) : IMediator
{
    public async Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResult));
        
        var handler = serviceFactory(handlerType) 
                      ?? throw new InvalidOperationException($"Handler for '{requestType.Name}' not found.");
        
        dynamic dynamicHandler = handler;
        dynamic dynamicRequest = request;
        
        return await dynamicHandler.HandleAsync(dynamicRequest, cancellationToken);
    }
    
    public async Task SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);

        var handler = serviceFactory(handlerType) 
                      ?? throw new InvalidOperationException($"Handler for '{requestType.Name}' not found.");

        dynamic dynamicHandler = handler;
        dynamic dynamicRequest = request;

        await dynamicHandler.HandleAsync(dynamicRequest, cancellationToken);
    }
}