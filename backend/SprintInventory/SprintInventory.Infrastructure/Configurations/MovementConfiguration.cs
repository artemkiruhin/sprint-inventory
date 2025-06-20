using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Infrastructure.Configurations;

public class MovementConfiguration : IEntityTypeConfiguration<InventoryItemMovementEntity>
{
    public void Configure(EntityTypeBuilder<InventoryItemMovementEntity> opt)
    {
        opt.HasKey(x => x.Id);
        opt.Property(x => x.Id).IsRequired();
        opt.HasIndex(x => x.Id).IsUnique();
            
        opt.Property(x => x.ItemId).IsRequired();
        opt.Property(x => x.Timestamp).IsRequired();
            
        opt.HasOne(x => x.User)
            .WithMany(x => x.Movements)
            .HasForeignKey(x => x.UserId);
        opt.HasOne(x => x.InventoryItem)
            .WithMany(x => x.Movements)
            .HasForeignKey(x => x.UserId);
        opt.HasOne(x => x.RoomFrom)
            .WithMany(x => x.MovementsFrom)
            .HasForeignKey(x => x.RoomFromId);
        opt.HasOne(x => x.RoomTo)
            .WithMany(x => x.MovementsIn)
            .HasForeignKey(x => x.RoomToId);
    }
}