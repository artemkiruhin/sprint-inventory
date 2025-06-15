using SprintInventory.Core.Interfaces;
using SprintInventory.Core.Interfaces.Services.Entity;
using SprintInventory.Core.Interfaces.Services.Mapper;
using SprintInventory.Core.Interfaces.Services.Security;
using SprintInventory.Core.Models.Contracts.Create;
using SprintInventory.Core.Models.Contracts.Delete;
using SprintInventory.Core.Models.Contracts.Specific;
using SprintInventory.Core.Models.Contracts.Update;
using SprintInventory.Core.Models.DTOs;
using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Services.EntityServices;

public class UserService : IUserService
{
    private readonly IUnitOfWork _database;
    private readonly IJwtService _jwtService;
    private readonly IUserMapper _mapper;

    public UserService(IUnitOfWork database, IJwtService jwtService, IUserMapper mapper)
    {
        _database = database;
        _jwtService = jwtService;
        _mapper = mapper;
    }
    
    public async Task<Result<Guid>> Create(UserCreateContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var sender = await _database.UserRepository.GetById(request.SenderId, ct);
            if (sender is null) return Result<Guid>.Failure("Invalid sender");
            if (!sender.IsAdmin) return Result<Guid>.Failure("Sender is not an admin");
            
            var usersByUsername = await _database.UserRepository.Search(x => x.Username == request.Username, ct);
            var userByUsername = usersByUsername.FirstOrDefault();
            if (userByUsername != null) return Result<Guid>.Failure("User already exists");

            if (request.Email != null)
            {
                var usersByEmail = await _database.UserRepository.Search(x => x.Email == request.Email, ct);
                var userByEmail = usersByEmail.FirstOrDefault();
                if (userByEmail != null) return Result<Guid>.Failure("User already exists");
            }

            var newUser = UserEntity.Create(
                request.Username,
                request.PasswordHash,
                request.Name,
                request.Surname,
                request.Patronymic,
                request.Email,
                request.IsAdmin
            );
            
            var createdUser = await _database.UserRepository.Create(newUser, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(createdUser.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error creating user: {e.Message}");
        }
    }

    public async Task<Result<Guid>> Update(UserUpdateContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var userById = await _database.UserRepository.GetById(request.Id, ct);
            if (userById is null) return Result<Guid>.Failure("User not found");
            
            var sender = await _database.UserRepository.GetById(request.SenderId, ct);
            if (sender is null) return Result<Guid>.Failure("Invalid sender");
            if (!sender.IsAdmin) return Result<Guid>.Failure("Sender is not an admin");

            if (request.Username != null)
            {
                var usersByUsername = await _database.UserRepository.Search(x => x.Username == request.Username, ct);
                var userByUsername = usersByUsername.FirstOrDefault();
                if (userByUsername != null) return Result<Guid>.Failure("User already exists");
                userById.Username = request.Username;
            }

            if (request.PasswordHash != null)
            {
                userById.PasswordHash = request.PasswordHash;
            }

            if (request.Name != null)
            {
                userById.Name = request.Name;
            }

            if (request.Surname != null)
            {
                userById.Surname = request.Surname;
            }

            if (request.Patronymic != null)
            {
                userById.Patronymic = request.Patronymic;
            }

            if (request.Patronymic != null && request.Patronymic == "")
            {
                userById.Patronymic = null;
            }
            
            if (request.Email != null)
            {
                var usersByEmail = await _database.UserRepository.Search(x => x.Email == request.Email, ct);
                var userByEmail = usersByEmail.FirstOrDefault();
                if (userByEmail != null) return Result<Guid>.Failure("User already exists");
                userById.Email = request.Email;
            }

            if (request.IsAdmin != null)
            {
                userById.IsAdmin = request.IsAdmin.Value;
            }
            

            var updatedUser = _database.UserRepository.Update(userById, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(updatedUser.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error updating user: {e.Message}");
        }
    }

    public async Task<Result<Guid>> Delete(BaseDeleteContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var userById = await _database.UserRepository.GetById(request.Id, ct);
            if (userById is null) return Result<Guid>.Failure("User not found");
            
            var sender = await _database.UserRepository.GetById(request.SenderId, ct);
            if (sender is null) return Result<Guid>.Failure("Invalid sender");
            if (!sender.IsAdmin) return Result<Guid>.Failure("Sender is not an admin");
            
            var deletedUser = await _database.UserRepository.Delete(request.Id, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(deletedUser.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error deleting user: {e.Message}");
        }
    }

    public async Task<Result<Guid>> Block(BaseDeleteContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var userById = await _database.UserRepository.GetById(request.Id, ct);
            if (userById is null) return Result<Guid>.Failure("User not found");
            
            var sender = await _database.UserRepository.GetById(request.SenderId, ct);
            if (sender is null) return Result<Guid>.Failure("Invalid sender");
            if (!sender.IsAdmin) return Result<Guid>.Failure("Sender is not an admin");
            
            userById.BlockedAt = DateTime.UtcNow;
            userById.IsBlocked = true;
            
            var updatedUser = _database.UserRepository.Update(userById, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(updatedUser.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error blocking user: {e.Message}");
        }
    }

    public async Task<Result<List<UserDetailedDTO>>> GetAllUsersDetailed(CancellationToken ct)
    {
        try
        {
            var users = await _database.UserRepository.GetAll(ct);
            var dtos = _mapper.MapToDetailedDTORange(users.ToList());
            return Result<List<UserDetailedDTO>>.Success(dtos);
        }
        catch (Exception e)
        {
            return Result<List<UserDetailedDTO>>.Failure($"Error getting all users: {e.Message}");
        }
    }

    public async Task<Result<List<UserShortDTO>>> GetAllUsersShort(CancellationToken ct)
    {
        try
        {
            var users = await _database.UserRepository.GetAll(ct);
            var dtos = _mapper.MapToShortDTORange(users.ToList());
            return Result<List<UserShortDTO>>.Success(dtos);
        }
        catch (Exception e)
        {
            return Result<List<UserShortDTO>>.Failure($"Error getting all users: {e.Message}");
        }
    }

    public async Task<Result<UserDetailedDTO>> GetUserById(Guid id, CancellationToken ct)
    {
        try
        {
            var user = await _database.UserRepository.GetById(id, ct);
            if (user is null) return Result<UserDetailedDTO>.Failure("User not found");
            var dto = _mapper.MapToDetailedDTO(user);
            return Result<UserDetailedDTO>.Success(dto);
        }
        catch (Exception e)
        {
            return Result<UserDetailedDTO>.Failure($"Error getting user: {e.Message}");
        }
    }

    public async Task<Result<LoginResponseContract>> Login(LoginRequestContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var usersByRequest = await _database.UserRepository.Search(x =>
                x.Username == request.Username && x.PasswordHash == request.PasswordHash, ct);
            var userByRequest = usersByRequest.FirstOrDefault();
            if (userByRequest is null) return Result<LoginResponseContract>.Failure("Invalid username or password");
            
            var jwtToken = _jwtService.GenerateToken(userByRequest.Id);
            if (!jwtToken.IsSuccess) return Result<LoginResponseContract>.Failure("Invalid jwt");
            
            var result = new LoginResponseContract(
                Id: userByRequest.Id,
                JwtToken: jwtToken.Data!
            );
            return Result<LoginResponseContract>.Success(result);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<LoginResponseContract>.Failure($"Error changing password: {e.Message}");
        }
    }

    public async Task<Result<Guid>> ChangePasswordAsUser(ChangePasswordAsUserContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var userByRequest = await _database.UserRepository.GetById(request.UserId, ct);
            if (userByRequest is null) return Result<Guid>.Failure("User not found");
            
            if (request.SenderId != request.UserId) return Result<Guid>.Failure("Invalid sender");
            
            if (request.OldPasswordHash == request.NewPasswordHash) return Result<Guid>.Failure("Invalid password");
            if (request.OldPasswordHash != userByRequest.PasswordHash) return Result<Guid>.Failure("Invalid password");
            if (request.NewPasswordHash == userByRequest.PasswordHash) return Result<Guid>.Failure("Invalid password");
            
            userByRequest.PasswordHash = request.NewPasswordHash;
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(userByRequest.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error changing password: {e.Message}");
        }
    }
}