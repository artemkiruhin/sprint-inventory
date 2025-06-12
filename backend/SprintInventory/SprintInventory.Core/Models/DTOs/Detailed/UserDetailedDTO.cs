namespace SprintInventory.Core.Models.DTOs.Detailed;

public record UserDetailedDTO(
    Guid Id,
    string Username,
    string PasswordHash,
    string Name,
    string Surname,
    string? Patronymic,
    string? Email,
    DateTime CreatedAt,
    bool IsAdmin,
    bool IsBlocked,
    DateTime? BlockedAt
);