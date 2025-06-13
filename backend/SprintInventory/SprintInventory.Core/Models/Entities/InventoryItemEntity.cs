namespace SprintInventory.Core.Models.Entities;

public class InventoryItemEntity
{
    private const uint NameMinLength = 2;
    private const uint NameMaxLength = 15;
    private const uint DescriptionMinLength = 2;
    
    
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
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
    public virtual ICollection<InventoryItemStatusLogEntity> StatusChanges { get; set; } = [];

    public static InventoryItemEntity Create(
        string name,
        string description,
        string? inventoryNumber,
        string? serialNumber,
        ItemStatus status,
        Guid? categoryId,
        Guid? roomId,
        Guid creatorId
    )
    {
        
        if (name.Trim().Length < NameMinLength || name.Trim().Length > NameMaxLength)
            throw new ArgumentOutOfRangeException(
                $"Name must be between {NameMinLength} and {NameMaxLength} characters long."
            );
        
        if (description.Trim().Length < NameMinLength)
            throw new ArgumentOutOfRangeException(
                $"Description must be more than {DescriptionMinLength} characters long."
            );
        
        return new()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            InventoryNumber = inventoryNumber,
            SerialNumber = serialNumber,
            Status = status,
            CategoryId = categoryId,
            RoomId = roomId,
            CreatedAt = DateTime.UtcNow,
            CreatorId = creatorId
        };
    } 
}