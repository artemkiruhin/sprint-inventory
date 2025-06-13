namespace SprintInventory.Core.Models.Entities;

public class InventoryItemMovementEntity
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid? RoomFromId { get; set; }
    public Guid? RoomToId { get; set; }
    public Guid? UserId { get; set; }
    public DateTime Timestamp { get; set; }
    
    public virtual UserEntity? User { get; set; } = null!;
    public virtual InventoryItemEntity InventoryItem { get; set; } = null!;
    public virtual RoomEntity? RoomFrom { get; set; } = null!;
    public virtual RoomEntity? RoomTo { get; set; } = null!;

    public static InventoryItemMovementEntity Create(Guid itemId, Guid? roomFromId, Guid? roomToId, Guid? userId)
    {
        if (!roomFromId.HasValue && !roomToId.HasValue) throw new ArgumentNullException("Room (from) Id and Room (to) Id cannot be null");
        
        return new()
        {
            Id = Guid.NewGuid(),
            ItemId = itemId,
            RoomFromId = roomFromId,
            RoomToId = roomToId,
            UserId = userId,
            Timestamp = DateTime.UtcNow
        };
    } 
}