namespace SprintInventory.Core.Models.Entities;

public class InventoryItemCreatingLogEntity
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid UserId { get; set; }
    
    public virtual UserEntity User { get; set; } = null!;
    public virtual InventoryItemEntity InventoryItem { get; set; } = null!;
}