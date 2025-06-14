namespace SprintInventory.Core.Models.Contracts.Create;

public record RoomCreateContract(
    string Name,
    string Address,
    Guid CreatorId
);