namespace SprintInventory.Core.Interfaces.Repositories.Base;

public interface IRepository<TEntity> where TEntity: class
{
    Task<Guid> Create(TEntity entity, CancellationToken ct);
    Task<Guid> Update(TEntity updatedEntity, CancellationToken ct);
    Task<Guid> Delete(Guid id, CancellationToken ct);
    Task<TEntity?> GetById(Guid id, CancellationToken ct);
    Task<IEnumerable<TEntity>> GetAll(CancellationToken ct);
}