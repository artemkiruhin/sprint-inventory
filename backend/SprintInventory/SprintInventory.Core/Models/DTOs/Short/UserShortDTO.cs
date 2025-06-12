namespace SprintInventory.Core.Models.DTOs.Short;

public record UserShortDTO(
    Guid Id,
    string Username,
    bool IsAdmin
);