using Microsoft.EntityFrameworkCore;
using SprintInventory.Core.Models.Entities;
using SprintInventory.Infrastructure.Configurations;

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
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoomConfiguration());
        modelBuilder.ApplyConfiguration(new StatusLogConfiguration());
        modelBuilder.ApplyConfiguration(new MovementConfiguration());
        modelBuilder.ApplyConfiguration(new ItemCreatingLogConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
    }
}