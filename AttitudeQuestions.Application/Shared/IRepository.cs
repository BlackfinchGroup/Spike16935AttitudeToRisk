namespace AttitudeQuestions.Application.Shared;

public interface IRepository<T>
{
    ValueTask<T?> Get(Guid primaryKey, CancellationToken cancellationToken = default);
    Task<T> Create(T entity, CancellationToken cancellationToken = default);
    Task Remove(T entity);
}
