using CQRS.Core.Commands;

namespace Post.Command.Infrastructure;

public class CommandDispatcher : ICommandDispatcher
{
    private const string errorTwice = "You cannot register the same command handler twice!";
    private const string errorNoHandler = "No command handler was registered!";
    private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = new();

    public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
    {
        if (_handlers.ContainsKey(typeof(T))) throw new IndexOutOfRangeException(errorTwice);

        _handlers.Add(typeof(T), x => handler((T)x));
    }

    public async Task SendAsync(BaseCommand command)
    {
        bool isHandler = _handlers.TryGetValue(command.GetType(), out Func<BaseCommand, Task>? handler);

        if (handler is null || !isHandler) throw new ArgumentNullException(nameof(handler), errorNoHandler);

        await handler(command);
    }
}