namespace SprintInventory.Core.Models.Entities;

public class InventoryItemStatusLogEntity
{
    public Guid Id { get; set; }
    public ItemStatus StatusFrom { get; set; }
    public ItemStatus StatusTo { get; set; }
    public Guid? UserId { get; set; }
    public DateTime Timestamp { get; set; }
    
    public virtual UserEntity User { get; set; } = null!;
}