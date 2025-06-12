using SprintInventory.Core.Models.DTOs.Short;

namespace SprintInventory.Core.Models.DTOs.Detailed;

public record CategoryDetailedDTO(
    Guid Id,
    string Name,
    string? Description,
    DateTime CreatedAt,
    Guid CreatorId,
    UserShortDTO Creator
);