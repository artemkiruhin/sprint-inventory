namespace SprintInventory.Core.Models.Entities;

public class InventoryItemEntity
{
    public Guid Id { get; set; }
    public string? InventoryNumber { get; set; }
    public string? SerialNumber { get; set; }
    public ItemStatus Status { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? RoomId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatorId { get; set; }
    
    public virtual UserEntity Creator { get; set; } = null!;
    public virtual RoomEntity? Room { get; set; } = null!;
    public virtual CategoryEntity? Category { get; set; } = null!;
    public virtual ICollection<InventoryItemMovementEntity> Movements { get; set; } = [];
    public virtual ICollection<InventoryItemCreatingLogEntity> CreatingLogs { get; set; } = [];
}