namespace SprintInventory.Core.Models.Entities;

public class CategoryEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatorId { get; set; }
    
    public virtual UserEntity Creator { get; set; } = null!;
    public virtual ICollection<InventoryItemEntity> InventoryItems { get; set; } = [];
}