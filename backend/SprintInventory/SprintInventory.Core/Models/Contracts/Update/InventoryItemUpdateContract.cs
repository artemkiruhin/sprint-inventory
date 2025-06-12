using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Core.Models.Contracts.Update;

public record InventoryItemUpdateContract (
    Guid Id,
    string? Name,
    string? Description,
    string? InventoryNumber,
    string? SerialNumber,
    ItemStatus? Status,
    Guid? CategoryId,
    Guid? RoomId,
    Guid UserId
);