namespace SprintInventory.Core.Models.Contracts.Update;

public record CategoryUpdateContract(
    Guid Id,
    string? Name, 
    string? Description,
    Guid UserId
);