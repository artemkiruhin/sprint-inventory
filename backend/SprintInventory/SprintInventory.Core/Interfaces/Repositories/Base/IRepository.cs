using System.Linq.Expressions;

namespace SprintInventory.Core.Interfaces.Repositories.Base;

public interface IRepository<TEntity> where TEntity: class
{
    Task<TEntity> Create(TEntity entity, CancellationToken ct);
    TEntity Update(TEntity updatedEntity, CancellationToken ct);
    Task<TEntity> Delete(Guid id, CancellationToken ct);
    Task<TEntity?> GetById(Guid id, CancellationToken ct);
    Task<IEnumerable<TEntity>> GetAll(CancellationToken ct);
    Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
}