using SprintInventory.Core.Interfaces.Repositories;
using SprintInventory.Core.Models.Entities;
using SprintInventory.Infrastructure.Repositories.Base;

namespace SprintInventory.Infrastructure.Repositories;

public class CreatingLogRepository(AppDbContext context) : BaseRepository<InventoryItemCreatingLogEntity>(context), ICreatingLogRepository { }