using SprintInventory.Core.Interfaces.Services.Mapper;
using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Services.Mappers;


public class UserMapper : IUserMapper
{
    public UserDetailedDTO MapToDetailedDTO(UserEntity userEntity)
    {
        var dto = new UserDetailedDTO(
            Id: userEntity.Id,
            Username: userEntity.Username,
            PasswordHash: userEntity.PasswordHash,
            Name: userEntity.Name,
            Surname: userEntity.Surname,
            Patronymic: userEntity.Patronymic,
            Email: userEntity.Email,
            CreatedAt: userEntity.CreatedAt,
            IsAdmin: userEntity.IsAdmin,
            IsBlocked: userEntity.IsBlocked,
            BlockedAt: userEntity.BlockedAt
        );
        return dto;
    }
    
    public List<UserDetailedDTO> MapToDetailedDTORange(List<UserEntity> userEntities)
    {
        var dtos = userEntities.Select(MapToDetailedDTO).ToList();
        return dtos;
    }
    
    public List<UserShortDTO> MapToShortDTORange(List<UserEntity> userEntities)
    {
        var dtos = userEntities.Select(MapToShortDTO).ToList();
        return dtos;
    }

    public UserShortDTO MapToShortDTO(UserEntity userEntity)
    {
        var dto = new UserShortDTO(
            Id: userEntity.Id,
            Username: userEntity.Username,
            IsAdmin: userEntity.IsAdmin
        );
        return dto;
    }
}