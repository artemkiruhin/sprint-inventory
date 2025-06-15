using SprintInventory.Core.Models.Contracts.Create;
using SprintInventory.Core.Models.Contracts.Delete;
using SprintInventory.Core.Models.Contracts.Specific;
using SprintInventory.Core.Models.Contracts.Update;
using SprintInventory.Core.Models.DTOs;
using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;

namespace SprintInventory.Core.Interfaces.Services.Entity;

public interface IInventoryItemService
{
    Task<Result<Guid>> Create(InventoryItemCreateContract request, CancellationToken ct);
    Task<Result<Guid>> Update(InventoryItemUpdateContract request, CancellationToken ct);
    Task<Result<Guid>> Delete(BaseDeleteContract request, CancellationToken ct);
    Task<Result<List<InventoryItemDetailedDTO>>> GetAllItemsDetailed(CancellationToken ct);
    Task<Result<List<InventoryItemShortDTO>>> GetAllItemsShort(CancellationToken ct);
    Task<Result<InventoryItemDetailedDTO>> GetItemById(Guid id, CancellationToken ct);
    Task<Result<Guid>> RemoveRoom(RemoveRoomInInventoryItemContract request, CancellationToken ct);
    Task<Result<Guid>> Move(MovementInventoryItemContract request, CancellationToken ct);
}