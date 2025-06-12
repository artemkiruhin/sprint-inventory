using SprintInventory.Core.Interfaces.Repositories;

namespace SprintInventory.Core.Interfaces;

public interface IUnitOfWork
{
    IRoomRepository RoomRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    ICreatingLogRepository CreatingLogRepository { get; }
    IInventoryItemRepository InventoryItemRepository { get; }
    IMovementRepository MovementRepository { get; }
    IStatusLogRepository StatusLogRepository { get; }
    IUserRepository UserRepository { get; }
    
    Task<int> SaveChangesAsync(CancellationToken ct);
    Task BeginTransactionAsync(CancellationToken ct);
    Task CommitTransactionAsync(CancellationToken ct);
    Task RollbackTransactionAsync(CancellationToken ct);
    
    void Dispose();
}