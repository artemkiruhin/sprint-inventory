using SprintInventory.Core.Interfaces.Repositories;
using SprintInventory.Core.Models.Entities;
using SprintInventory.Infrastructure.Repositories.Base;

namespace SprintInventory.Infrastructure.Repositories;

public class MovementRepository(AppDbContext context) : BaseRepository<InventoryItemMovementEntity>(context), IMovementRepository { }