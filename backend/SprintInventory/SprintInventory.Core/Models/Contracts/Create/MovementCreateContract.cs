namespace SprintInventory.Core.Models.Contracts.Create;

public record MovementCreateContract(
    Guid ItemId,
    Guid RoomFromId,
    Guid RoomToId,
    Guid CreatorId
);