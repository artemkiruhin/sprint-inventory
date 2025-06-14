namespace SprintInventory.Core.Models.Contracts.Create;

public record UserCreateContract(
    string Username,
    string PasswordHash,
    string Name,
    string Surname,
    string? Patronymic,
    string? Email,
    bool IsAdmin,
    Guid SenderId
);