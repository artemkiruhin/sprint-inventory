using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Infrastructure.Configurations;

public class StatusLogConfiguration : IEntityTypeConfiguration<InventoryItemStatusLogEntity>
{
    public void Configure(EntityTypeBuilder<InventoryItemStatusLogEntity> opt)
    {
        opt.HasKey(x => x.Id);
        opt.Property(x => x.Id).IsRequired();
        opt.HasIndex(x => x.Id).IsUnique();
            
        opt.Property(x => x.ItemId).IsRequired();
        opt.Property(x => x.StatusFrom).IsRequired();
        opt.Property(x => x.StatusTo).IsRequired();
        opt.Property(x => x.Timestamp).IsRequired();
            
        opt.HasOne(x => x.User)
            .WithMany(x => x.InventoryItemStatusLogs)
            .HasForeignKey(x => x.UserId);
        opt.HasOne(x => x.Item)
            .WithMany(x => x.StatusChanges)
            .HasForeignKey(x => x.UserId);
    }
}