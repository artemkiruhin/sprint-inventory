using SprintInventory.Core.Models.DTOs.Short;

namespace SprintInventory.Core.Models.DTOs.Detailed;

public record RoomDetailedDTO (
    Guid Id,
    string Name,
    string Address,
    DateTime CreatedAt,
    UserShortDTO Creator
);