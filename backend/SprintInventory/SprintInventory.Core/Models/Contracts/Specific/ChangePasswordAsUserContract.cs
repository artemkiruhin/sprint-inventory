namespace SprintInventory.Core.Models.Contracts.Specific;

public record ChangePasswordAsUserContract(
    Guid UserId,
    Guid SenderId,
    string OldPasswordHash,
    string NewPasswordHash
);