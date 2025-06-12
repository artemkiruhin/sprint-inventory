namespace SprintInventory.Core.Models.Entities;

public class RoomEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatorId { get; set; }
    
    public virtual UserEntity Creator { get; set; } = null!;
    public virtual ICollection<InventoryItemEntity> InventoryItems { get; set; } = [];
    public virtual ICollection<InventoryItemMovementEntity> MovementsIn { get; set; } = [];
    public virtual ICollection<InventoryItemMovementEntity> MovementsFrom { get; set; } = [];
}