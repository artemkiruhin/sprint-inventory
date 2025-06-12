using SprintInventory.Core.Models.DTOs.Short;

namespace SprintInventory.Core.Models.DTOs.Detailed;

public record InventoryItemDetailedDTO(
    Guid Id,
    string Name,
    string Description,
    string? InventoryNumber,
    string? SerialNumber,
    string Status,
    DateTime CreatedAt,
    UserShortDTO Creator,
    RoomShortDTO? Room,
    CategoryShortDTO? Category
);