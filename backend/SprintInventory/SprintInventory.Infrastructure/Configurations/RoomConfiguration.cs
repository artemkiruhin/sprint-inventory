using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Infrastructure.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<RoomEntity>
{
    public void Configure(EntityTypeBuilder<RoomEntity> opt)
    {
        opt.HasKey(x => x.Id);
        opt.Property(x => x.Id).IsRequired();
        opt.HasIndex(x => x.Id).IsUnique();
            
        opt.Property(x => x.Name).IsRequired().HasMaxLength(RoomEntity.NameMaxLength);
        opt.Property(x => x.Address);
        opt.Property(x => x.CreatedAt).IsRequired();
        opt.Property(x => x.CreatorId).IsRequired();
            
        opt.HasIndex(x => x.Name).IsUnique();
            
        opt.HasOne(x => x.Creator)
            .WithMany(x => x.Rooms)
            .HasForeignKey(x => x.CreatorId);
        opt.HasMany(x => x.InventoryItems)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId);
        opt.HasMany(x => x.MovementsIn)
            .WithOne(x => x.RoomTo)
            .HasForeignKey(x => x.RoomToId);
        opt.HasMany(x => x.MovementsFrom)
            .WithOne(x => x.RoomFrom)
            .HasForeignKey(x => x.RoomFromId);
    }
}