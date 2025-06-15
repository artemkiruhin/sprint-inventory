namespace SprintInventory.Core.Interfaces.Services.Mapper;

public interface IMapper<TEntity, TDto>
{
    TDto MapDto(TEntity entity);
    TEntity MapEntity(TDto dto);
}