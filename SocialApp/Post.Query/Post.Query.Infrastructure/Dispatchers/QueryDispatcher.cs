using CQRS.Core.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrastructure.Dispatchers;

public class QueryDispatcher : IQueryDispatcher<PostDb>
{
    private readonly Dictionary<Type, Func<BaseQuery, Task<List<PostDb>>>> _handlers = new();

    public void RegisterHandler<TQuery>(Func<TQuery, Task<List<PostDb>>> handler) where TQuery : BaseQuery
    {
        if (_handlers.ContainsKey(typeof(TQuery)))
        {
            throw new IndexOutOfRangeException("You cannot register the same query handler twice!");
        }

        _handlers.Add(typeof(TQuery), x => handler((TQuery)x));
    }

    public async Task<List<PostDb>> SendAsync(BaseQuery query)
    {
        if (_handlers.TryGetValue(query.GetType(), out Func<BaseQuery, Task<List<PostDb>>> handler))
        {
            return await handler(query);
        }

        throw new ArgumentNullException(nameof(handler), "No query handler was registered!");
    }
}