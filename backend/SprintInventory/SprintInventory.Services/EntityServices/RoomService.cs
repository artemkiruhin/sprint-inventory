using SprintInventory.Core.Interfaces;
using SprintInventory.Core.Interfaces.Services.Entity;
using SprintInventory.Core.Interfaces.Services.Mapper;
using SprintInventory.Core.Models.Contracts.Create;
using SprintInventory.Core.Models.Contracts.Delete;
using SprintInventory.Core.Models.Contracts.Update;
using SprintInventory.Core.Models.DTOs;
using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Services.EntityServices;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _database;
    private readonly IRoomMapper _mapper;

    public RoomService(IUnitOfWork database, IRoomMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }
    
    public async Task<Result<Guid>> Create(RoomCreateContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            if (string.IsNullOrEmpty(request.Name.Trim())) return Result<Guid>.Failure("Room name cannot be empty");
            var roomsByRequest = await _database.RoomRepository.Search(x => x.Name == request.Name, ct);
            var roomByRequest = roomsByRequest.FirstOrDefault();
            if (roomByRequest != null) return Result<Guid>.Failure("Room already exists");
            
            var creatorByRequest = await _database.UserRepository.GetById(request.CreatorId, ct);
            if (creatorByRequest != null) return Result<Guid>.Failure("User already exists");
            if (!creatorByRequest!.IsAdmin) return Result<Guid>.Failure("User does not have admin rights");
            
            var newRoom = RoomEntity.Create(request.Name, request.Address, request.CreatorId);
            var createdRoom = await _database.RoomRepository.Create(newRoom, ct);
            
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(createdRoom.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error creating room: {e.Message}");
        }
    }

    public async Task<Result<Guid>> Update(RoomUpdateContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var room = await _database.RoomRepository.GetById(request.Id, ct);
            if (room == null) return Result<Guid>.Failure("Room not found");
            
            if (string.IsNullOrEmpty(request.Name.Trim())) return Result<Guid>.Failure("Room name cannot be empty");
            var roomsByRequest = await _database.RoomRepository.Search(x => x.Name == request.Name, ct);
            var roomByRequest = roomsByRequest.FirstOrDefault();
            if (roomByRequest != null) return Result<Guid>.Failure("Room already exists");
            
            var creatorByRequest = await _database.UserRepository.GetById(request.UserId, ct);
            if (creatorByRequest != null) return Result<Guid>.Failure("User already exists");
            if (!creatorByRequest!.IsAdmin) return Result<Guid>.Failure("User does not have admin rights");

            if (request.Name != null && !string.IsNullOrEmpty(request.Name.Trim())) room.Name = request.Name.Trim();
            if (request.Address != null && !string.IsNullOrEmpty(request.Address.Trim())) room.Address = request.Address;
            
            var createdRoom = _database.RoomRepository.Update(room, ct);
            
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(createdRoom.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error updating room: {e.Message}");
        }
    }

    public async Task<Result<Guid>> Delete(BaseDeleteContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var existRoom = await _database.RoomRepository.GetById(request.Id, ct);
            if (existRoom == null) return Result<Guid>.Failure("Room not found");
            
            var userByRequest = await _database.UserRepository.GetById(request.SenderId, ct);
            if (userByRequest == null) return Result<Guid>.Failure("User not found");
            if (!userByRequest.IsAdmin) return Result<Guid>.Failure("Action is not allowed for non admin users");
            
            var deletedRoom = await _database.RoomRepository.Delete(request.Id, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(deletedRoom.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error deleting room: {e.Message}");
        }
    }

    public async Task<Result<List<RoomDetailedDTO>>> GetAllRoomsDetailed(CancellationToken ct)
    {
        try
        {
            var rooms = await _database.RoomRepository.GetAll(ct);
            var dtos = _mapper.MapToDetailedDTORange(rooms.ToList());
            return Result<List<RoomDetailedDTO>>.Success(dtos);
        }
        catch (Exception e)
        {
            return Result<List<RoomDetailedDTO>>.Failure($"Error getting all rooms: {e.Message}");
        }
    }

    public async Task<Result<List<RoomShortDTO>>> GetAllRoomsShort(CancellationToken ct)
    {
        try
        {
            var rooms = await _database.RoomRepository.GetAll(ct);
            var dtos = _mapper.MapToShortDTORange(rooms.ToList());
            return Result<List<RoomShortDTO>>.Success(dtos);
        }
        catch (Exception e)
        {
            return Result<List<RoomShortDTO>>.Failure($"Error getting all rooms: {e.Message}");
        }
    }

    public async Task<Result<RoomDetailedDTO>> GetRoomById(Guid id, CancellationToken ct)
    {
        try
        {
            var room = await _database.RoomRepository.GetById(id, ct);
            if (room == null) return Result<RoomDetailedDTO>.Failure("Room not found");
            var dtos = _mapper.MapToDetailedDTO(room);
            return Result<RoomDetailedDTO>.Success(dtos);
        }
        catch (Exception e)
        {
            return Result<RoomDetailedDTO>.Failure($"Error getting room: {e.Message}");
        }
    }
}