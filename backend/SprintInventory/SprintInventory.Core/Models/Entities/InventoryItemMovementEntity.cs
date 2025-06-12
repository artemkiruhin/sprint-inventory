namespace SprintInventory.Core.Models.Entities;

public class InventoryItemMovementEntity
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid? RoomFromId { get; set; }
    public Guid? RoomToId { get; set; }
    public Guid? UserId { get; set; }
    public DateTime Timestamp { get; set; }
    
    public virtual UserEntity User { get; set; } = null!;
    public virtual InventoryItemEntity InventoryItem { get; set; } = null!;
    public virtual RoomEntity? RoomFrom { get; set; } = null!;
    public virtual RoomEntity? RoomTo { get; set; } = null!;
}