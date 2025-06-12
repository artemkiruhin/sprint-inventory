using SprintInventory.Core.Models.DTOs.Short;

namespace SprintInventory.Core.Models.DTOs.Detailed;

public record MovementDetailedDTO (
    Guid Id,
    Guid ItemId,
    DateTime Timestamp,
    
    RoomShortDTO? RoomFrom,
    RoomShortDTO? RoomTo,
    UserShortDTO? User
);