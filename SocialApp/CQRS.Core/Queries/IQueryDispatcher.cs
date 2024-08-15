namespace CQRS.Core.Queries;

public interface IQueryDispatcher<TEntity>
{
    void RegisterHandler<TQuery>(Func<TQuery, Task<List<TEntity>>> handler) where TQuery : Query;
    Task<List<TEntity>> SendAsync(Query query);
}
