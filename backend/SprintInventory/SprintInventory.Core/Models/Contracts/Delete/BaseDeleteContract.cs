namespace SprintInventory.Core.Models.Contracts.Delete;

public record BaseDeleteContract(
    Guid Id,
    Guid SenderId
);