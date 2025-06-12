namespace SprintInventory.Core.Models.Entities;

public class InventoryItemStatusLogEntity
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public ItemStatus StatusFrom { get; set; }
    public ItemStatus StatusTo { get; set; }
    public Guid? UserId { get; set; }
    public DateTime Timestamp { get; set; }
    
    public virtual UserEntity User { get; set; } = null!;
    public virtual InventoryItemEntity Item { get; set; } = null!;

    public static InventoryItemStatusLogEntity Create(Guid itemId, ItemStatus statusFrom, ItemStatus statusTo, Guid? userId)
        => new()
        {
            Id = Guid.NewGuid(),
            ItemId = itemId,
            StatusFrom = statusFrom,
            StatusTo = statusTo,
            UserId = userId,
            Timestamp = DateTime.UtcNow
        };
}