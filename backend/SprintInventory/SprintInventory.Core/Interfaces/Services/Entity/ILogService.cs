using SprintInventory.Core.Models.Contracts.Create;
using SprintInventory.Core.Models.DTOs;
using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;

namespace SprintInventory.Core.Interfaces.Services.Entity;

public interface ILogService
{
    Task<Result<Guid>> LogStatusChange(StatusLogCreateContract request, CancellationToken ct);
    Task<Result<Guid>> LogMovement(MovementCreateContract request, CancellationToken ct);
    Task<Result<Guid>> LogCreating(CreatingLogCreateContract request, CancellationToken ct);
    
    Task<Result<List<CreatingLogDetailedDTO>>> GetAllCreatingLogs(CancellationToken ct);
    Task<Result<List<MovementDetailedDTO>>> GetAllMovements(CancellationToken ct);
    Task<Result<List<StatusLogDetailedDTO>>> GetAllStatusChanges(CancellationToken ct);
    
    Task<Result<List<CreatingLogShortDTO>>> GetCreatingLogShort(CancellationToken ct);
}