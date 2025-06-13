using SprintInventory.Core.Models.Contracts.Create;
using SprintInventory.Core.Models.Contracts.Delete;
using SprintInventory.Core.Models.Contracts.Specific;
using SprintInventory.Core.Models.Contracts.Update;
using SprintInventory.Core.Models.DTOs;
using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;

namespace SprintInventory.Core.Interfaces.Services.Entity;

public interface IUserService
{
    Task<Result<Guid>> Create(UserCreateContract request, CancellationToken ct);
    Task<Result<Guid>> Update(UserUpdateContract request, CancellationToken ct);
    Task<Result<Guid>> Delete(BaseDeleteContract request, CancellationToken ct);
    Task<Result<Guid>> Block(BaseDeleteContract request, CancellationToken ct);
    Task<Result<List<UserDetailedDTO>>> GetAllUsersDetailed(CancellationToken ct);
    Task<Result<List<UserShortDTO>>> GetAllUsersShort(CancellationToken ct);
    Task<Result<UserDetailedDTO>> GetUserById(Guid id, CancellationToken ct);
    Task<Result<LoginResponseContract>> Login(LoginRequestContract request, CancellationToken ct);
}