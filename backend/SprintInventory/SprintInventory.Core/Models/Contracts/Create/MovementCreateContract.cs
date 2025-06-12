namespace SprintInventory.Core.Models.Contracts.Create;

public record MovementCreateContract(
    Guid ItemId,
    Guid RoomToId,
    Guid CreatorId
);