namespace SprintInventory.Core.Models.Contracts.Update;

public record RoomUpdateContract(
    Guid Id,
    string? Name,
    string? Address,
    Guid UserId
);