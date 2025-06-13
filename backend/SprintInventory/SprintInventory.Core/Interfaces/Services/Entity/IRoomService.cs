using SprintInventory.Core.Models.Contracts.Create;
using SprintInventory.Core.Models.Contracts.Delete;
using SprintInventory.Core.Models.Contracts.Specific;
using SprintInventory.Core.Models.Contracts.Update;
using SprintInventory.Core.Models.DTOs;
using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;

namespace SprintInventory.Core.Interfaces.Services.Entity;

public interface IRoomService
{
    Task<Result<Guid>> Create(RoomCreateContract request, CancellationToken ct);
    Task<Result<Guid>> Update(RoomUpdateContract request, CancellationToken ct);
    Task<Result<Guid>> Delete(BaseDeleteContract request, CancellationToken ct);
    Task<Result<List<RoomDetailedDTO>>> GetAllRoomsDetailed(CancellationToken ct);
    Task<Result<List<RoomShortDTO>>> GetAllRoomsShort(CancellationToken ct);
    Task<Result<RoomDetailedDTO>> GetRoomById(Guid id, CancellationToken ct);
}