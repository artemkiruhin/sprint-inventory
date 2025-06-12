namespace SprintInventory.Core.Models.Contracts.Create;

public record CreatingLogCreateContract(
    Guid ItemId, 
    Guid CreatorId
);