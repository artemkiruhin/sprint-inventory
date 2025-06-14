using Microsoft.EntityFrameworkCore.Infrastructure;
using SprintInventory.Core.Interfaces;
using SprintInventory.Core.Interfaces.Services.Entity;
using SprintInventory.Core.Interfaces.Services.Extension;
using SprintInventory.Core.Models.Contracts.Create;
using SprintInventory.Core.Models.DTOs;
using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Services.EntityServices;

public class LogService : ILogService
{
    private readonly IUnitOfWork _database;
    private readonly IItemStatusExtensionService _statusExt;

    public LogService(IUnitOfWork database, IItemStatusExtensionService statusExt)
    {
        _database = database;
        _statusExt = statusExt;
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
            
            var roomFromByRequest = await _database.RoomRepository.GetById(request.RoomFromId, ct);
            if (roomFromByRequest == null) return Result<Guid>.Failure("Room does not exist");
            
            var roomToByRequest = await _database.RoomRepository.GetById(request.RoomToId, ct);
            if (roomToByRequest == null) return Result<Guid>.Failure("Room does not exist");
            
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
            var dtos = logs.Select(entity => new CreatingLogDetailedDTO(
                Id: entity.Id,
                ItemId: entity.ItemId,
                UserId: entity.UserId,
                User: new UserShortDTO(
                    Id: entity.UserId,
                    Username: entity.User.Username,
                    IsAdmin: entity.User.IsAdmin
                ),
                InventoryItem: new InventoryItemShortDTO(
                    Id: entity.InventoryItem.Id,
                    Name: entity.InventoryItem.Name,
                    Description: entity.InventoryItem.Description,
                    InventoryNumber: entity.InventoryItem.InventoryNumber,
                    SerialNumber: entity.InventoryItem.SerialNumber,
                    Status: _statusExt.GetStringName(entity.InventoryItem.Status),
                    CreatorId: entity.InventoryItem.CreatorId,
                    RoomName: entity.InventoryItem.Room?.Name ?? "Нет кабинета",
                    Category: entity.InventoryItem.Category != null
                        ? new CategoryShortDTO(
                            Id: entity.InventoryItem.Category.Id,
                            Name: entity.InventoryItem.Category.Name,
                            Description: entity.InventoryItem.Category.Description
                        )
                        : null,
                    CreatedAt: entity.InventoryItem.CreatedAt
                )
            )).ToList();
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
            var dtos = logs.Select(entity => new MovementDetailedDTO(
                Id: entity.Id,
                ItemId: entity.ItemId,
                Timestamp: entity.Timestamp,
                RoomFrom: entity.RoomFrom != null
                    ? new RoomShortDTO(
                        Id: entity.RoomFrom.Id,
                        Name: entity.RoomFrom.Name,
                        Address: entity.RoomFrom.Address
                    )
                    : null,
                RoomTo: entity.RoomTo != null
                    ? new RoomShortDTO(
                        Id: entity.RoomTo.Id,
                        Name: entity.RoomTo.Name,
                        Address: entity.RoomTo.Address
                    )
                    : null,
                User: entity.User != null
                    ? new UserShortDTO(
                        Id: entity.User.Id,
                        Username: entity.User.Username,
                        IsAdmin: entity.User.IsAdmin
                    )
                    : null
            )).ToList();
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
            var dtos = logs.Select(entity => new StatusLogDetailedDTO(
                Id: entity.Id,
                StatusFrom: _statusExt.GetStringName(entity.StatusFrom),
                StatusTo: _statusExt.GetStringName(entity.StatusTo),
                Timestamp: entity.Timestamp,
                User: entity.User != null
                    ? new UserShortDTO(
                        Id: entity.User.Id,
                        Username: entity.User.Username,
                        IsAdmin: entity.User.IsAdmin
                    )
                    : null
            )).ToList();
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
            var dtos = logs.Select(entity => new CreatingLogShortDTO(
                Id: entity.Id,
                ItemId: entity.ItemId,
                UserId: entity.UserId,
                ItemNumber: entity.InventoryItem.InventoryNumber ?? "Нет номера",
                Username: entity.User.Username
            )).ToList();
            return Result<List<CreatingLogShortDTO>>.Success(dtos);
        }
        catch (Exception e)
        {
            return Result<List<CreatingLogShortDTO>>.Failure($"Error getting all creating logs: {e.Message}");
        }
    }
}