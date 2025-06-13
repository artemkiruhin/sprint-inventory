using Microsoft.EntityFrameworkCore;
using SprintInventory.Core.Interfaces.Repositories;
using SprintInventory.Core.Models.Entities;
using SprintInventory.Infrastructure.Repositories.Base;

namespace SprintInventory.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<UserEntity>(context), IUserRepository
{
    public async Task<IEnumerable<UserEntity>> GetBlockedUsers(CancellationToken ct)
    {
        return await DbSet.Where(u => u.IsBlocked).ToListAsync(ct);
    }

    public async Task<IEnumerable<UserEntity>> GetActiveUsers(CancellationToken ct)
    {
        return await DbSet.Where(u => !u.IsBlocked).ToListAsync(ct);
    }

    public async Task<IEnumerable<UserEntity>> GetAdmins(CancellationToken ct)
    {
        return await DbSet.Where(u => u.IsAdmin).ToListAsync(ct);
    }

    public async Task<IEnumerable<UserEntity>> GetNonAdmins(CancellationToken ct)
    {
        return await DbSet.Where(u => !u.IsAdmin).ToListAsync(ct);
    }
}