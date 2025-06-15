using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Core.Interfaces.Services.Mapper;

public interface IUserMapper
{
    UserDetailedDTO MapToDetailedDTO(UserEntity userEntity);
    List<UserDetailedDTO> MapToDetailedDTORange(List<UserEntity> userEntities);
    List<UserShortDTO> MapToShortDTORange(List<UserEntity> userEntities);
    UserShortDTO MapToShortDTO(UserEntity userEntity);
}