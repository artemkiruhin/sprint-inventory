using SprintInventory.Core.Interfaces;
using SprintInventory.Core.Interfaces.Services.Entity;
using SprintInventory.Core.Interfaces.Services.Extension;
using SprintInventory.Core.Interfaces.Services.Mapper;
using SprintInventory.Core.Models.Contracts.Create;
using SprintInventory.Core.Models.Contracts.Delete;
using SprintInventory.Core.Models.Contracts.Specific;
using SprintInventory.Core.Models.Contracts.Update;
using SprintInventory.Core.Models.DTOs;
using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Services.EntityServices;

public class InventoryItemService : IInventoryItemService
{
    private readonly IUnitOfWork _database;
    private readonly ILogService _logger;
    private readonly IInventoryItemMapper _mapper;

    public InventoryItemService(IUnitOfWork database, ILogService logger, IInventoryItemMapper mapper)
    {
        _database = database;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<Result<Guid>> Create(InventoryItemCreateContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            if (request.InventoryNumber != null)
            {
                var itemsByInventoryNumber = await _database.InventoryItemRepository
                    .Search(x => x.InventoryNumber == request.InventoryNumber, ct);
                var itemByInventoryNumber = itemsByInventoryNumber.FirstOrDefault();
                if (itemByInventoryNumber != null) return Result<Guid>.Failure("Inventory number already exists");
            }

            if (request.CategoryId != null)
            {
                var categoryByRequest = await _database.CategoryRepository.GetById(request.CategoryId.Value, ct);
                if (categoryByRequest == null) return Result<Guid>.Failure("Category doesn't exist");
            }

            if (request.RoomId != null)
            {
                var roomByRequest = await _database.RoomRepository.GetById(request.RoomId.Value, ct);
                if (roomByRequest == null) return Result<Guid>.Failure("Room doesn't exist");
            }
            
            var creatorByRequest = await _database.UserRepository.GetById(request.CreatorId, ct);
            if (creatorByRequest == null) return Result<Guid>.Failure("Creator doesn't exist");

            var newItem = InventoryItemEntity.Create(request.Name,
                request.Description,
                request.InventoryNumber,
                request.SerialNumber,
                request.Status,
                request.CategoryId,
                request.RoomId,
                request.CreatorId
            );
            var createdItem = await _database.InventoryItemRepository.Create(newItem, ct);
            await _database.SaveChangesAsync(ct);

            var logResult = await _logger.LogCreating(
                new CreatingLogCreateContract(createdItem.Id, request.CreatorId),
                ct
            );
            
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(createdItem.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error creating item: {e.Message}");
        }
    }

    public async Task<Result<Guid>> Update(InventoryItemUpdateContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var item = await _database.InventoryItemRepository.GetById(request.Id, ct);
            if (item == null) return Result<Guid>.Failure("Item doesn't exist");

            if (request.Name != null)
            {
                item.Name = request.Name;
            }

            if (request.Description != null)
            {
                item.Description = request.Description;
            }

            if (request.InventoryNumber != null)
            {
                var itemsByInventoryNumber = await _database.InventoryItemRepository.Search(x => x.InventoryNumber == request.InventoryNumber, ct);
                var itemByInventoryNumber = itemsByInventoryNumber.FirstOrDefault();
                if (itemByInventoryNumber != null) return Result<Guid>.Failure("Inventory number already exists");
                item.InventoryNumber = request.InventoryNumber;
            }

            if (request.SerialNumber != null)
            {
                item.SerialNumber = request.SerialNumber;
            }

            if (request.Status != null)
            {
                item.Status = request.Status.Value;
            }

            if (request.CategoryId != null)
            {
                var itemsByCategoryId = await _database.CategoryRepository.GetById(request.CategoryId.Value, ct);
                if (itemsByCategoryId == null) return Result<Guid>.Failure("Category doesn't exist");
                item.CategoryId = request.CategoryId.Value;
            }

            if (request.RoomId != null)
            {
                var itemsByRoomId = await _database.RoomRepository.GetById(request.RoomId.Value, ct);
                if (itemsByRoomId == null) return Result<Guid>.Failure("Room doesn't exist");
                item.RoomId = request.RoomId.Value;
            }
            
            var userByRequest = await _database.UserRepository.GetById(request.UserId, ct);
            if (userByRequest == null) return Result<Guid>.Failure("User doesn't exist");
            
            var updatedItem = _database.InventoryItemRepository.Update(item, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(updatedItem.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error updating item: {e.Message}");
        }
    }

    public async Task<Result<Guid>> Delete(BaseDeleteContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var existItem = await _database.InventoryItemRepository.GetById(request.Id, ct);
            if (existItem == null) return Result<Guid>.Failure("Item not found");
            
            var userByRequest = await _database.UserRepository.GetById(request.SenderId, ct);
            if (userByRequest == null) return Result<Guid>.Failure("User not found");
            
            var deletedItem = await _database.InventoryItemRepository.Delete(request.Id, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(deletedItem.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error deleting item: {e.Message}");
        }
    }

    public async Task<Result<List<InventoryItemDetailedDTO>>> GetAllItemsDetailed(CancellationToken ct)
    {
        try
        {
            var items = await _database.InventoryItemRepository.GetAll(ct);
            var dtos = _mapper.MapToDetailedDTORange(items.ToList());
            return Result<List<InventoryItemDetailedDTO>>.Success(dtos);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<List<InventoryItemDetailedDTO>>.Failure($"Error getting item details: {e.Message}");
        }
    }

    public async Task<Result<List<InventoryItemShortDTO>>> GetAllItemsShort(CancellationToken ct)
    {
        try
        {
            var items = await _database.InventoryItemRepository.GetAll(ct);
            var dtos = _mapper.MapToShortDTORange(items.ToList());
            return Result<List<InventoryItemShortDTO>>.Success(dtos);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<List<InventoryItemShortDTO>>.Failure($"Error getting items: {e.Message}");
        }
    }

    public async Task<Result<InventoryItemDetailedDTO>> GetItemById(Guid id, CancellationToken ct)
    {
        try
        {
            var item = await _database.InventoryItemRepository.GetById(id, ct);
            if (item == null) return Result<InventoryItemDetailedDTO>.Failure("Item not found");
            var dto = _mapper.MapToDetailedDTO(item);
            return Result<InventoryItemDetailedDTO>.Success(dto);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<InventoryItemDetailedDTO>.Failure($"Error getting item by id details: {e.Message}");
        }
    }

    public async Task<Result<Guid>> RemoveRoom(RemoveRoomInInventoryItemContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var existItem = await _database.InventoryItemRepository.GetById(request.ItemId, ct);
            if (existItem == null) return Result<Guid>.Failure("Item not found");
            
            var userByRequest = await _database.UserRepository.GetById(request.UserId, ct);
            if (userByRequest == null) return Result<Guid>.Failure("User not found");
            
            var roomFromId = existItem.RoomId;
            existItem.Room = null;
            
            var updatedItem = _database.InventoryItemRepository.Update(existItem, ct);
            await _database.SaveChangesAsync(ct);
            
            var logResult = await _logger.LogMovement(new MovementCreateContract(updatedItem.Id, roomFromId, null, request.UserId), ct);
            if (!logResult.IsSuccess) return Result<Guid>.Failure(logResult.ErrorMessage ?? "Failed to create inventory item");
            
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(updatedItem.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error removing room of item: {e.Message}");
        }
    }

    public async Task<Result<Guid>> Move(MovementInventoryItemContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var item = await _database.InventoryItemRepository.GetById(request.ItemId, ct);
            if (item == null) return Result<Guid>.Failure("Item not found");
            
            var userByRequest = await _database.UserRepository.GetById(request.UserId, ct);
            if (userByRequest == null) return Result<Guid>.Failure("User not found");
            
            var roomFromId = item.Room?.Id;
            item.RoomId = request.RoomId;
            
            await _database.SaveChangesAsync(ct);
            
            var logResult = await _logger.LogMovement(new MovementCreateContract(request.ItemId, roomFromId, request.RoomId, request.UserId), ct);
            if (!logResult.IsSuccess) return Result<Guid>.Failure(logResult.ErrorMessage ?? "Failed to create inventory item");
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(item.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error moving item: {e.Message}");
        }
    }
}