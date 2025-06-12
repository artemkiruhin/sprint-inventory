namespace SprintInventory.Core.Models.DTOs.Short;

public record CategoryShortDTO(
    Guid Id,
    string Name,
    string? Description
);