using SprintInventory.Core.Models.DTOs.Short;

namespace SprintInventory.Core.Models.DTOs.Detailed;

public record CreatingLogDetailedDTO(
    Guid Id,
    Guid ItemId,
    Guid UserId,
    UserShortDTO User,
    InventoryItemShortDTO InventoryItem
);