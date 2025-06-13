using SprintInventory.Core.Models.Contracts.Create;
using SprintInventory.Core.Models.Contracts.Delete;
using SprintInventory.Core.Models.Contracts.Update;
using SprintInventory.Core.Models.DTOs;
using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;

namespace SprintInventory.Core.Interfaces.Services.Entity;

public interface ICategoryService
{
    Task<Result<Guid>> Create(CategoryCreateContract request, CancellationToken ct);
    Task<Result<Guid>> Update(CategoryUpdateContract request, CancellationToken ct);
    Task<Result<Guid>> Delete(BaseDeleteContract request, CancellationToken ct);
    Task<Result<List<CategoryDetailedDTO>>> GetAllCategoriesDetailed(CancellationToken ct);
    Task<Result<List<CategoryShortDTO>>> GetAllCategoriesShort(CancellationToken ct);
    Task<Result<CategoryDetailedDTO>> GetCategoryById(Guid id, CancellationToken ct);
}