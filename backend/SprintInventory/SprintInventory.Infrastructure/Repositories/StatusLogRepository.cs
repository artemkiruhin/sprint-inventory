using SprintInventory.Core.Interfaces.Repositories;
using SprintInventory.Core.Models.Entities;
using SprintInventory.Infrastructure.Repositories.Base;

namespace SprintInventory.Infrastructure.Repositories;

public class StatusLogRepository(AppDbContext context) : BaseRepository<InventoryItemStatusLogEntity>(context), IStatusLogRepository { }