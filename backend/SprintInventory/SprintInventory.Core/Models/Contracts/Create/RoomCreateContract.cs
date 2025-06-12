namespace SprintInventory.Core.Models.Contracts.Create;

public record RoomCreateContract(
    string Name,
    string Address,
    DateTime CreatedAt,
    Guid CreatorId
);