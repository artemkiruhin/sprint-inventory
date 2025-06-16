using SprintInventory.Core.Interfaces.Services.Mapper;
using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Services.Mappers;

public class CategoryMapper : ICategoryMapper
{
    private readonly IUserMapper _userMapper;

    public CategoryMapper(IUserMapper userMapper)
    {
        _userMapper = userMapper;
    }
    
    public CategoryShortDTO MapToCategoryShortDTO(CategoryEntity categoryEntity)
    {
        var dto = new CategoryShortDTO(
            Id: categoryEntity.Id,
            Name: categoryEntity.Name,
            Description: categoryEntity.Description
        );
        return dto;
    }

    public List<CategoryShortDTO> MapToCategoryShortDTORange(List<CategoryEntity> categoryEntities)
    {
        var dtos = categoryEntities.Select(MapToCategoryShortDTO).ToList();
        return dtos;
    }

    public CategoryDetailedDTO MapToCategoryDetailedDTO(CategoryEntity categoryEntity)
    {
        var dto = new CategoryDetailedDTO(
            Id: categoryEntity.Id,
            Name: categoryEntity.Name,
            Description: categoryEntity.Description,
            CreatedAt: categoryEntity.CreatedAt,
            CreatorId: categoryEntity.CreatorId,
            Creator: _userMapper.MapToShortDTO(categoryEntity.Creator)
        );
        return dto;
    }

    public List<CategoryDetailedDTO> MapToCategoryDetailedDTORange(List<CategoryEntity> categoryEntities)
    {
        var dtos = categoryEntities.Select(MapToCategoryDetailedDTO).ToList();
        return dtos;
    }
}