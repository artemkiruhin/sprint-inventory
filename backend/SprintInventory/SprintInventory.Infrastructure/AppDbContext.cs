using Microsoft.EntityFrameworkCore;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RoomEntity> Rooms { get; set; }
    public DbSet<InventoryItemCreatingLogEntity> CreatingLogs { get; set; }
    public DbSet<InventoryItemMovementEntity> Movements { get; set; }
    public DbSet<InventoryItemEntity> Items { get; set; }
    public DbSet<InventoryItemStatusLogEntity> StatusLogs { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(opt =>
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
        });
        
        modelBuilder.Entity<RoomEntity>(opt =>
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
        });
        
        modelBuilder.Entity<InventoryItemStatusLogEntity>(opt =>
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
        });
        
        modelBuilder.Entity<InventoryItemMovementEntity>(opt =>
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
                .HasForeignKey(x => x.UserId);
            opt.HasOne(x => x.RoomTo)
                .WithMany(x => x.MovementsFrom)
                .HasForeignKey(x => x.UserId);
        });
        
        modelBuilder.Entity<InventoryItemCreatingLogEntity>(opt =>
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
        });
        
        modelBuilder.Entity<CategoryEntity>(opt =>
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
        });
    }
}