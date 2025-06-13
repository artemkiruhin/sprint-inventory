using SprintInventory.Core.Interfaces;
using SprintInventory.Core.Interfaces.Repositories;

namespace SprintInventory.Infrastructure;

public class UnitOfWork(
    IRoomRepository roomRepository,
    ICategoryRepository categoryRepository,
    ICreatingLogRepository creatingLogRepository,
    IInventoryItemRepository inventoryItemRepository,
    IMovementRepository movementRepository,
    IStatusLogRepository statusLogRepository,
    IUserRepository userRepository,
    AppDbContext context)
    : IUnitOfWork
{
    public IRoomRepository RoomRepository { get; } = roomRepository;
    public ICategoryRepository CategoryRepository { get; } = categoryRepository;
    public ICreatingLogRepository CreatingLogRepository { get; } = creatingLogRepository;
    public IInventoryItemRepository InventoryItemRepository { get; } = inventoryItemRepository;
    public IMovementRepository MovementRepository { get; } = movementRepository;
    public IStatusLogRepository StatusLogRepository { get; } = statusLogRepository;
    public IUserRepository UserRepository { get; } = userRepository;
    private AppDbContext Context { get; } = context;


    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await Context.SaveChangesAsync(ct);
    }

    public async Task BeginTransactionAsync(CancellationToken ct)
    {
        await Context.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct)
    {
        await Context.Database.CommitTransactionAsync(ct);
    }

    public async Task RollbackTransactionAsync(CancellationToken ct)
    {
        await Context.Database.RollbackTransactionAsync(ct);
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}