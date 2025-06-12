using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Core.Models.Contracts.Create;

public record InventoryItemCreateContract (
    string Name,
    string Description,
    string? InventoryNumber,
    string? SerialNumber,
    ItemStatus Status,
    Guid? CategoryId,
    Guid? RoomId,
    Guid CreatorId
);