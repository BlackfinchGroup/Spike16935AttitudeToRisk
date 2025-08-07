using AttitudeQuestions.Application.Shared;
using AttitudeQuestions.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace AttitudeQuestions.Infrastructure.Shared.Persistence;

public class BaseCrudRepository<T>(DbContext dbContext) : IRepository<T> where T : AggregateRoot<Guid>
{
    public virtual async Task<T> Create(T entity, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<T>().AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual async ValueTask<T?> Get(Guid primaryKey, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.Set<T>().FindAsync([primaryKey], cancellationToken);
        if (entity is null)
        {
            return null;
        }
        var dbEntry = dbContext.Entry(entity);
        if (dbEntry.Property<bool>("IsDeleted").CurrentValue)
        {
            return null;
        }

        return entity;
    }

    public virtual Task Remove(T entity)
    {
        var entry = dbContext.Update(entity);
        var createdAtPropertyEntry = entry.Property("DeletedAt");
        createdAtPropertyEntry.CurrentValue = DateTime.UtcNow;
        var isDeletedPropertyEntry = entry.Property("IsDeleted");
        isDeletedPropertyEntry.CurrentValue = true;

        return Task.CompletedTask;
    }

    protected IQueryable<T> AsQueryable(bool allowDeleted = false)
    {
        return dbContext.Set<T>()
            .Where(u => allowDeleted || EF.Property<bool>(u, "IsDeleted") == false);
    }

}

public class BaseQueryRepository<T>(DbContext dbContext) where T : AggregateRoot<Guid>
{
    protected IQueryable<T> AsQueryable()
    {
        return dbContext.Set<T>();
    }

    public async Task<T?> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return await AsQueryable().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> Exists(Guid id, CancellationToken cancellationToken = default)
    {
        return await AsQueryable().AnyAsync(x => x.Id == id, cancellationToken);
    }
}
