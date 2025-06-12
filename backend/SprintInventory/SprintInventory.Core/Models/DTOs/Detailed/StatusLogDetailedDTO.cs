using SprintInventory.Core.Models.DTOs.Short;

namespace SprintInventory.Core.Models.DTOs.Detailed;

public record StatusLogDetailedDTO (
    Guid Id,
    string StatusFrom,
    string StatusTo,
    DateTime Timestamp,
    UserShortDTO? User
);