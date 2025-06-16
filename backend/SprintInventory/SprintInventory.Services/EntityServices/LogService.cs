using Microsoft.EntityFrameworkCore.Infrastructure;
using SprintInventory.Core.Interfaces;
using SprintInventory.Core.Interfaces.Services.Entity;
using SprintInventory.Core.Interfaces.Services.Extension;
using SprintInventory.Core.Interfaces.Services.Mapper;
using SprintInventory.Core.Models.Contracts.Create;
using SprintInventory.Core.Models.DTOs;
using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Services.EntityServices;

public class LogService : ILogService
{
    private readonly IUnitOfWork _database;
    private readonly ILogMapper _mapper;

    public LogService(IUnitOfWork database, ILogMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }
    
    public async Task<Result<Guid>> LogStatusChange(StatusLogCreateContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var itemByRequest = await _database.InventoryItemRepository.GetById(request.ItemId, ct);
            if (itemByRequest == null) return Result<Guid>.Failure("Item does not exist");
            
            var creatorByRequest = await _database.UserRepository.GetById(request.CreatorId, ct);
            if (creatorByRequest == null) return Result<Guid>.Failure("Creator does not exist");
            
            var newLog = InventoryItemStatusLogEntity.Create(request.ItemId, request.StatusFrom, request.StatusTo, request.CreatorId);
            var createdLog = await _database.StatusLogRepository.Create(newLog, ct);
            
            await _database.CommitTransactionAsync(ct);
            
            return Result<Guid>.Success(createdLog.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error creating log: {e.Message}");
        }
    }

    public async Task<Result<Guid>> LogMovement(MovementCreateContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var itemByRequest = await _database.InventoryItemRepository.GetById(request.ItemId, ct);
            if (itemByRequest == null) return Result<Guid>.Failure("Item does not exist");

            if (!request.RoomFromId.HasValue && !request.RoomToId.HasValue) return Result<Guid>.Failure("No room from or room to found");
            
            if (request.RoomFromId.HasValue)
            {
                var roomFromByRequest = await _database.RoomRepository.GetById(request.RoomFromId.Value, ct);
                if (roomFromByRequest == null) return Result<Guid>.Failure("Room does not exist");
            }

            if (request.RoomToId.HasValue)
            {
                var roomToByRequest = await _database.RoomRepository.GetById(request.RoomToId.Value, ct);
                if (roomToByRequest == null) return Result<Guid>.Failure("Room does not exist");
            }
            
            var creatorByRequest = await _database.UserRepository.GetById(request.CreatorId, ct);
            if (creatorByRequest == null) return Result<Guid>.Failure("Creator does not exist");
            
            var newLog = InventoryItemMovementEntity.Create(request.ItemId, request.RoomFromId, request.RoomToId, request.CreatorId);
            var createdLog = await _database.MovementRepository.Create(newLog, ct);
            
            await _database.CommitTransactionAsync(ct);
            
            return Result<Guid>.Success(createdLog.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error creating log: {e.Message}");
        }
    }

    public async Task<Result<Guid>> LogCreating(CreatingLogCreateContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var itemByRequest = await _database.InventoryItemRepository.GetById(request.ItemId, ct);
            if (itemByRequest == null) return Result<Guid>.Failure("Item does not exist");
            
            var creatorByRequest = await _database.UserRepository.GetById(request.CreatorId, ct);
            if (creatorByRequest == null) return Result<Guid>.Failure("Creator does not exist");
            
            var newLog = InventoryItemCreatingLogEntity.Create(request.ItemId, request.CreatorId);
            var createdLog = await _database.CreatingLogRepository.Create(newLog, ct);
            
            await _database.CommitTransactionAsync(ct);
            
            return Result<Guid>.Success(createdLog.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error creating log: {e.Message}");
        }
    }

    public async Task<Result<List<CreatingLogDetailedDTO>>> GetAllCreatingLogs(CancellationToken ct)
    {
        try
        {
            var logs = await _database.CreatingLogRepository.GetAll(ct);
            var dtos = _mapper.MapCreatingLogDetailedDTORange(logs.ToList());
            return Result<List<CreatingLogDetailedDTO>>.Success(dtos);
        }
        catch (Exception e)
        {
            return Result<List<CreatingLogDetailedDTO>>.Failure($"Error getting all creating logs: {e.Message}");
        }
    }

    public async Task<Result<List<MovementDetailedDTO>>> GetAllMovements(CancellationToken ct)
    {
        try
        {
            var logs = await _database.MovementRepository.GetAll(ct);
            var dtos = _mapper.MapMovementDetailedDTORange(logs.ToList());
            return Result<List<MovementDetailedDTO>>.Success(dtos);
        }
        catch (Exception e)
        {
            return Result<List<MovementDetailedDTO>>.Failure($"Error getting all movements: {e.Message}");
        }
    }

    public async Task<Result<List<StatusLogDetailedDTO>>> GetAllStatusChanges(CancellationToken ct)
    {
        try
        {
            var logs = await _database.StatusLogRepository.GetAll(ct);
            var dtos = _mapper.MapStatusLogDetailedDTORange(logs.ToList());
            return Result<List<StatusLogDetailedDTO>>.Success(dtos);
        }
        catch (Exception e)
        {
            return Result<List<StatusLogDetailedDTO>>.Failure($"Error getting all status logs: {e.Message}");
        }
    }

    public async Task<Result<List<CreatingLogShortDTO>>> GetCreatingLogShort(CancellationToken ct)
    {
        try
        {
            var logs = await _database.CreatingLogRepository.GetAll(ct);
            var dtos = _mapper.MapCreatingLogShortDTORange(logs.ToList());
            return Result<List<CreatingLogShortDTO>>.Success(dtos);
        }
        catch (Exception e)
        {
            return Result<List<CreatingLogShortDTO>>.Failure($"Error getting all creating logs: {e.Message}");
        }
    }
}