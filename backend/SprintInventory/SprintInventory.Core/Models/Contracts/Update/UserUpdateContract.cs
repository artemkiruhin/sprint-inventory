namespace SprintInventory.Core.Models.Contracts.Update;

public record UserUpdateContract(
    Guid Id,
    string? Username,
    string? PasswordHash,
    string? Name,
    string? Surname,
    string? Patronymic,
    string? Email,
    bool? IsAdmin,
    Guid SenderId
);