using SprintInventory.Core.Interfaces.Repositories.Base;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Core.Interfaces.Repositories;

public interface IUserRepository : IRepository<UserEntity>
{
    Task<IEnumerable<UserEntity>> GetBlockedUsers(CancellationToken ct);
    Task<IEnumerable<UserEntity>> GetActiveUsers(CancellationToken ct);
    Task<IEnumerable<UserEntity>> GetAdmins(CancellationToken ct);
    Task<IEnumerable<UserEntity>> GetNonAdmins(CancellationToken ct);
}