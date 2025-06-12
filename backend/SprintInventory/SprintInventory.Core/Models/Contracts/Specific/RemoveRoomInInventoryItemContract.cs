namespace SprintInventory.Core.Models.Contracts.Specific;

public record RemoveRoomInInventoryItemContract(
    Guid ItemId,
    Guid UserId
);