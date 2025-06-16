using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Core.Interfaces.Services.Mapper;

public interface ICategoryMapper
{
    CategoryShortDTO MapToCategoryShortDTO(CategoryEntity categoryEntity);
    List<CategoryShortDTO> MapToCategoryShortDTORange(List<CategoryEntity> categoryEntities);
    CategoryDetailedDTO MapToCategoryDetailedDTO(CategoryEntity categoryEntity);
    List<CategoryDetailedDTO> MapToCategoryDetailedDTORange(List<CategoryEntity> categoryEntities);
}