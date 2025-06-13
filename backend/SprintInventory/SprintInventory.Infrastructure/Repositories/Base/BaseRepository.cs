using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SprintInventory.Core.Interfaces.Repositories.Base;

namespace SprintInventory.Infrastructure.Repositories.Base;

public class BaseRepository<TEntity>(AppDbContext context) : IRepository<TEntity> where TEntity : class
{
    protected DbSet<TEntity> DbSet { get; } = context.Set<TEntity>();

    public async Task<TEntity> Create(TEntity entity, CancellationToken ct)
    {
        var result = await DbSet.AddAsync(entity, ct);
        return result.Entity;
    }

    public TEntity Update(TEntity updatedEntity, CancellationToken ct)
    {
        var result = DbSet.Update(updatedEntity);
        return result.Entity;
    }

    public async Task<TEntity> Delete(Guid id, CancellationToken ct)
    {
        var entity = await DbSet.FindAsync(keyValues: [id], cancellationToken: ct);
        if (entity == null) throw new KeyNotFoundException();
        var result = DbSet.Remove(entity);
        return result.Entity;
    }

    public async Task<TEntity?> GetById(Guid id, CancellationToken ct)
    {
        return await DbSet.FindAsync(keyValues: [id], cancellationToken: ct);
    }

    public async Task<IEnumerable<TEntity>> GetAll(CancellationToken ct)
    {
        return await DbSet.ToListAsync(cancellationToken: ct);
    }

    public async Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
    {
        return await DbSet.Where(predicate).ToListAsync(cancellationToken: ct);
    }
}