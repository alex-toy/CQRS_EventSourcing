using CQRS.Core.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrastructure.Dispatchers;

public class QueryDispatcher<T> : IQueryDispatcher<T> where T : Entity
{
    private readonly Dictionary<Type, Func<BaseQuery, Task<List<T>>>> _handlers = new();

    public void RegisterHandler<TQuery>(Func<TQuery, Task<List<T>>> handler) where TQuery : BaseQuery
    {
        if (_handlers.ContainsKey(typeof(TQuery)))
        {
            throw new IndexOutOfRangeException("You cannot register the same query handler twice!");
        }

        _handlers.Add(typeof(TQuery), x => handler((TQuery)x));
    }

    public async Task<List<T>> SendAsync(BaseQuery query)
    {
        Type queryType = query.GetType();
        if (_handlers.TryGetValue(queryType, out Func<BaseQuery, Task<List<T>>> handler))
        {
            return await handler(query);
        }

        throw new ArgumentNullException(nameof(handler), "No query handler was registered!");
    }
}