namespace SprintInventory.Core.Models.DTOs.Short;

public record CreatingLogShortDTO(
    Guid Id,
    Guid ItemId,
    string ItemNumber,
    Guid UserId,
    string Username
);