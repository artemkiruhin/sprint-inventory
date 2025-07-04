﻿namespace SprintInventory.Core.Models.Entities;

public class InventoryItemCreatingLogEntity
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid UserId { get; set; }
    public DateTime Timestamp { get; set; }
    
    public virtual UserEntity User { get; set; } = null!;
    public virtual InventoryItemEntity InventoryItem { get; set; } = null!;

    public static InventoryItemCreatingLogEntity Create(Guid itemId, Guid userId)
        => new()
        {
            Id = Guid.NewGuid(),
            ItemId = itemId,
            UserId = userId,
            Timestamp = DateTime.UtcNow
        };
}