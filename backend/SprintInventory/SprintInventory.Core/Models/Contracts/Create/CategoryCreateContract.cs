namespace SprintInventory.Core.Models.Contracts.Create;

public record CategoryCreateContract(
    string Name, 
    string? Description,
    Guid CreatorId
);