namespace SprintInventory.Core.Models.Entities;

public class RoomEntity
{
    public const int NameMinLength = 3;
    public const int NameMaxLength = 20;
    public const int AddressMinLength = 5;
    
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatorId { get; set; }
    
    public virtual UserEntity Creator { get; set; } = null!;
    public virtual ICollection<InventoryItemEntity> InventoryItems { get; set; } = [];
    public virtual ICollection<InventoryItemMovementEntity> MovementsIn { get; set; } = [];
    public virtual ICollection<InventoryItemMovementEntity> MovementsFrom { get; set; } = [];

    public static RoomEntity Create(string name, string address, Guid creatorId)
    {
        if (string.IsNullOrEmpty(name.Trim())) throw new ArgumentNullException("Name cannot be null or empty.");
        if (name.Trim().Length < NameMinLength || name.Trim().Length > NameMaxLength)
            throw new ArgumentOutOfRangeException($"Name must be between {NameMinLength} and {NameMaxLength} characters long.");

        if (string.IsNullOrEmpty(address)) throw new ArgumentNullException("Address cannot be null or empty.");
        if (address.Trim().Length < AddressMinLength) 
            throw new ArgumentOutOfRangeException($"Address must be more than {AddressMinLength} characters long.");
        
        return new()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Address = address,
            CreatedAt = DateTime.UtcNow,
            CreatorId = creatorId
        };
    }
}