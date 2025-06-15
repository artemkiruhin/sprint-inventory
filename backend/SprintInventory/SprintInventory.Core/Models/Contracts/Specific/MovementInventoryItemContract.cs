namespace SprintInventory.Core.Models.Contracts.Specific;

public record MovementInventoryItemContract(
    Guid ItemId,
    Guid? RoomId,
    Guid UserId
);