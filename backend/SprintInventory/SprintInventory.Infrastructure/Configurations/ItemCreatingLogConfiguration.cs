using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Infrastructure.Configurations;

public class ItemCreatingLogConfiguration : IEntityTypeConfiguration<InventoryItemCreatingLogEntity>
{
    public void Configure(EntityTypeBuilder<InventoryItemCreatingLogEntity> opt)
    {
        opt.HasKey(x => x.Id);
        opt.Property(x => x.Id).IsRequired();
        opt.HasIndex(x => x.Id).IsUnique();
            
        opt.Property(x => x.ItemId).IsRequired();
        opt.Property(x => x.UserId).IsRequired();
        opt.Property(x => x.Timestamp).IsRequired();
            
        opt.HasOne(x => x.User)
            .WithMany(x => x.CreatingLogs)
            .HasForeignKey(x => x.UserId);
        opt.HasOne(x => x.InventoryItem)
            .WithMany(x => x.CreatingLogs)
            .HasForeignKey(x => x.UserId);
    }
}