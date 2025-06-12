namespace SprintInventory.Core.Models.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public string? Patronymic { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime BlockedAt { get; set; }

    public virtual ICollection<RoomEntity> Rooms { get; set; } = [];
    public virtual ICollection<CategoryEntity> Categories { get; set; } = [];
    public virtual ICollection<InventoryItemEntity> InventoryItems { get; set; } = [];
    public virtual ICollection<InventoryItemStatusLogEntity> InventoryItemStatusLogs { get; set; } = [];
    public virtual ICollection<InventoryItemMovementEntity> Movements { get; set; } = [];
    public virtual ICollection<InventoryItemCreatingLogEntity> CreatingLogs { get; set; } = [];
}