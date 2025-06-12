namespace SprintInventory.Core.Models.Entities;

public class CategoryEntity
{
    private const uint NameMinLength = 3;
    private const uint NameMaxLength = 20;
    private const uint DescriptionMinLength = 2;
    
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatorId { get; set; }
    
    public virtual UserEntity Creator { get; set; } = null!;
    public virtual ICollection<InventoryItemEntity> InventoryItems { get; set; } = [];

    public static CategoryEntity Create(string name, string? description, Guid creatorId)
    {
        if (name.Trim().Length < NameMinLength || name.Trim().Length > NameMaxLength)
            throw new ArgumentOutOfRangeException(
                $"Name must be between {NameMinLength} and {NameMaxLength} characters long."
            );
        
        if (!string.IsNullOrWhiteSpace(description.Trim()) && description.Length < DescriptionMinLength)
            throw new ArgumentOutOfRangeException($"Description must be more than {DescriptionMinLength} characters long.");
        
        return new()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            CreatorId = creatorId
        };
    }
}