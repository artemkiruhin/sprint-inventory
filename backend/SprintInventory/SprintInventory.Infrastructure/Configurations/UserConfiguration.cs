using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> opt)
    {
        opt.HasKey(x => x.Id);
        opt.Property(x => x.Id).IsRequired();
        opt.HasIndex(x => x.Id).IsUnique();

        opt.Property(x => x.Username).IsRequired().HasMaxLength(UserEntity.UsernameMaxLength);
        opt.Property(x => x.PasswordHash).IsRequired();
        opt.Property(x => x.Name).IsRequired().HasMaxLength(UserEntity.NameMaxLength);
        opt.Property(x => x.Surname).HasMaxLength(UserEntity.SurnameMaxLength);
        opt.Property(x => x.CreatedAt).IsRequired();
        opt.Property(x => x.IsAdmin).IsRequired();
        opt.Property(x => x.IsBlocked).IsRequired();

        opt.HasIndex(x => x.Username).IsUnique();
        opt.HasIndex(x => x.Email).IsUnique();

        opt.HasMany(x => x.Rooms)
            .WithOne(x => x.Creator)
            .HasForeignKey(x => x.CreatorId);
        opt.HasMany(x => x.Categories)
            .WithOne(x => x.Creator)
            .HasForeignKey(x => x.CreatorId);
        opt.HasMany(x => x.InventoryItems)
            .WithOne(x => x.Creator)
            .HasForeignKey(x => x.CreatorId);
        opt.HasMany(x => x.InventoryItemStatusLogs)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
        opt.HasMany(x => x.Movements)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
        opt.HasMany(x => x.CreatingLogs)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}