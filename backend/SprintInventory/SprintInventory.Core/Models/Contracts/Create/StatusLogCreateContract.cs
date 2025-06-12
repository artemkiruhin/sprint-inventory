using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Core.Models.Contracts.Create;

public record StatusLogCreateContract(
    Guid ItemId,
    ItemStatus StatusFrom,
    ItemStatus StatusTo,
    Guid CreatorId
);