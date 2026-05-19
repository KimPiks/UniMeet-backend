using Microsoft.Extensions.DependencyInjection;
using UniMeet.Shared.Abstractions;
using UniMeet.Shared.Mediator;
using UniMeet.Shared.Mediator.Extensions;

namespace UniMeet.UnitTests.Cqrs;

public class MediatorTests
{
    [Fact]
    public async Task SendAsync_With_result_dispatches_to_matching_query_handler()
    {
        var handler = new TestQueryHandler();
        var mediator = new Mediator(type => type == typeof(IRequestHandler<TestQuery, string>) ? handler : null!);
        using var cts = new CancellationTokenSource();

        var result = await mediator.SendAsync(new TestQuery("Ada"), cts.Token);

        Assert.Equal("Hello Ada", result);
        Assert.Equal("Ada", handler.HandledName);
        Assert.Equal(cts.Token, handler.CancellationToken);
    }

    [Fact]
    public async Task SendAsync_Without_result_dispatches_to_matching_command_handler()
    {
        var handler = new TestCommandHandler();
        var mediator = new Mediator(type => type == typeof(IRequestHandler<TestCommand>) ? handler : null!);

        await mediator.SendAsync(new TestCommand("recorded"));

        Assert.Equal("recorded", handler.HandledValue);
    }

    [Fact]
    public async Task SendAsync_When_handler_is_missing_throws_clear_exception()
    {
        var mediator = new Mediator(_ => null!);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            mediator.SendAsync(new TestQuery("Ada")));

        Assert.Equal("Handler for 'TestQuery' not found.", exception.Message);
    }

    [Fact]
    public async Task RegisterMediator_registers_command_and_query_handlers_from_assembly()
    {
        var services = new ServiceCollection();
        services.AddScoped<HandlerProbe>();
        services.RegisterMediator(typeof(MediatorTests).Assembly);
        await using var provider = services.BuildServiceProvider();

        var mediator = provider.GetRequiredService<IMediator>();
        var queryResult = await mediator.SendAsync(new RegisteredQuery(21));
        await mediator.SendAsync(new RegisteredCommand("cqrs"));
        var probe = provider.GetRequiredService<HandlerProbe>();

        Assert.Equal(42, queryResult);
        Assert.Equal("cqrs", probe.LastCommandValue);
    }

    public sealed record TestQuery(string Name) : IQuery<string>;

    public sealed class TestQueryHandler : IQueryHandler<TestQuery, string>
    {
        public string? HandledName { get; private set; }
        public CancellationToken CancellationToken { get; private set; }

        public Task<string> HandleAsync(TestQuery request, CancellationToken cancellationToken = default)
        {
            HandledName = request.Name;
            CancellationToken = cancellationToken;
            return Task.FromResult($"Hello {request.Name}");
        }
    }

    public sealed record TestCommand(string Value) : ICommand;

    public sealed class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public string? HandledValue { get; private set; }

        public Task HandleAsync(TestCommand request, CancellationToken cancellationToken = default)
        {
            HandledValue = request.Value;
            return Task.CompletedTask;
        }
    }
}

public sealed record RegisteredQuery(int Value) : IQuery<int>;

public sealed class RegisteredQueryHandler : IQueryHandler<RegisteredQuery, int>
{
    public Task<int> HandleAsync(RegisteredQuery request, CancellationToken cancellationToken = default)
        => Task.FromResult(request.Value * 2);
}

public sealed record RegisteredCommand(string Value) : ICommand;

public sealed class RegisteredCommandHandler(HandlerProbe probe) : ICommandHandler<RegisteredCommand>
{
    public Task HandleAsync(RegisteredCommand request, CancellationToken cancellationToken = default)
    {
        probe.LastCommandValue = request.Value;
        return Task.CompletedTask;
    }
}

public sealed class HandlerProbe
{
    public string? LastCommandValue { get; set; }
}
