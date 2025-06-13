using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Infrastructure.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> opt)
    {
        opt.HasKey(x => x.Id);
        opt.Property(x => x.Id).IsRequired();
        opt.HasIndex(x => x.Id).IsUnique();
            
        opt.Property(x => x.Name).IsRequired();
        opt.Property(x => x.CreatedAt).IsRequired();
        opt.Property(x => x.CreatorId).IsRequired();

        opt.HasOne(x => x.Creator)
            .WithMany(x => x.Categories)
            .HasForeignKey(x => x.CreatorId);
        opt.HasMany(x => x.InventoryItems)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryId);
    }
}