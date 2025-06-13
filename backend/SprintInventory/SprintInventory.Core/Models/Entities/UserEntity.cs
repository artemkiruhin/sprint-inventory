namespace SprintInventory.Core.Models.Entities;

public class UserEntity
{
    public const int UsernameMinLength = 4;
    public const int UsernameMaxLength = 15;
    public const int NameMinLength = 2;
    public const int NameMaxLength = 15;
    public const int SurnameMinLength = 2;
    public const int SurnameMaxLength = 20;
    
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
    public DateTime? BlockedAt { get; set; }

    public virtual ICollection<RoomEntity> Rooms { get; set; } = [];
    public virtual ICollection<CategoryEntity> Categories { get; set; } = [];
    public virtual ICollection<InventoryItemEntity> InventoryItems { get; set; } = [];
    public virtual ICollection<InventoryItemStatusLogEntity> InventoryItemStatusLogs { get; set; } = [];
    public virtual ICollection<InventoryItemMovementEntity> Movements { get; set; } = [];
    public virtual ICollection<InventoryItemCreatingLogEntity> CreatingLogs { get; set; } = [];

    public static UserEntity Create(
        string username,
        string passwordHash,
        string name,
        string surname,
        string? patronymic,
        string? email,
        bool isAdmin
    )
    {
        if (string.IsNullOrEmpty(username.Trim())) throw new ArgumentNullException("Username cannot be null or empty.");
        if (username.Trim().Length < UsernameMinLength || username.Trim().Length > UsernameMaxLength)
            throw new ArgumentOutOfRangeException($"Username must be between {UsernameMinLength} and {UsernameMaxLength} characters long.");

        if (string.IsNullOrEmpty(passwordHash.Trim())) throw new ArgumentNullException("Password hash cannot be null or empty.");
        if (string.IsNullOrEmpty(name.Trim())) throw new ArgumentNullException("Name hash cannot be null or empty.");
        if (string.IsNullOrEmpty(surname.Trim())) throw new ArgumentNullException("Surname hash cannot be null or empty.");

        if (name.Trim().Length < NameMinLength || username.Trim().Length > NameMaxLength)
            throw new ArgumentOutOfRangeException(
                $"Name must be between {NameMinLength} and {NameMaxLength} characters long."
            );
        
        if (surname.Trim().Length < SurnameMinLength || surname.Trim().Length > SurnameMinLength)
            throw new ArgumentOutOfRangeException(
                $"Surname must be between {SurnameMinLength} and {SurnameMinLength} characters long."
            );

        return new()
        {
            Id = Guid.NewGuid(),
            Username = username,
            PasswordHash = passwordHash,
            Name = name,
            Surname = surname,
            Patronymic = patronymic,
            Email = email,
            IsAdmin = isAdmin,
            CreatedAt = DateTime.UtcNow,
            IsBlocked = false,
            BlockedAt = null,
        };
    }
}