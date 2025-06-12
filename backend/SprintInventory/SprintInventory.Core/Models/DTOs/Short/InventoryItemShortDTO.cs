namespace SprintInventory.Core.Models.DTOs.Short;

public record InventoryItemShortDTO(
    Guid Id,
    string Name,
    string Description,
    string? InventoryNumber,
    string? SerialNumber,
    string Status,
    Guid CreatorId,
    string RoomName,
    CategoryShortDTO? Category,
    DateTime CreatedAt
);