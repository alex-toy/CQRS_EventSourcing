﻿namespace CQRS.Core.Queries;

public interface IQueryDispatcher<TEntity>
{
    void RegisterHandler<TQuery>(Func<TQuery, Task<List<TEntity>>> handler) where TQuery : BaseQuery;
    Task<List<TEntity>> HandleAsync(BaseQuery query);
}
