using SprintInventory.Core.Interfaces;
using SprintInventory.Core.Interfaces.Services.Entity;
using SprintInventory.Core.Models.Contracts.Create;
using SprintInventory.Core.Models.Contracts.Delete;
using SprintInventory.Core.Models.Contracts.Update;
using SprintInventory.Core.Models.DTOs;
using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Services.EntityServices;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _database;

    public CategoryService(IUnitOfWork database)
    {
        _database = database;
    }
    
    public async Task<Result<Guid>> Create(CategoryCreateContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            if (string.IsNullOrEmpty(request.Name.Trim())) return Result<Guid>.Failure("Category name cannot be empty");
            var existCategoriesByName = await _database.CategoryRepository.Search(x => x.Name == request.Name.Trim(), ct);
            var existCategoryByName = existCategoriesByName.FirstOrDefault();
            if (existCategoryByName != null) return Result<Guid>.Failure("Category already exists");
            var creatorByRequest = await _database.UserRepository.GetById(request.CreatorId, ct);
            if (creatorByRequest == null) return Result<Guid>.Failure("Creator not found");
            if (!creatorByRequest.IsAdmin) return Result<Guid>.Failure("Action is not allowed for non admin users");
            
            var newCategory = CategoryEntity.Create(request.Name.Trim(), request.Description, request.CreatorId);
            var createdCategory = await _database.CategoryRepository.Create(newCategory, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(createdCategory.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error creating category: {e.Message}");
        }
    }

    public async Task<Result<Guid>> Update(CategoryUpdateContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var existCategory = await _database.CategoryRepository.GetById(request.Id, ct);
            if (existCategory == null) return Result<Guid>.Failure("Category not found");
            
            if (string.IsNullOrEmpty(request.Name) && string.IsNullOrEmpty(request.Description))
                return Result<Guid>.Failure("Category name or Description cannot be empty");

            var userByRequest = await _database.UserRepository.GetById(request.UserId, ct);
            if (userByRequest == null) return Result<Guid>.Failure("User not found");
            if (!userByRequest.IsAdmin) return Result<Guid>.Failure("Action is not allowed for non admin users");
            
            if (!string.IsNullOrEmpty(request.Name))
            {
                var existCategoriesByName = await _database.CategoryRepository.Search(x => x.Name == request.Name.Trim(), ct);
                var existCategoryByName = existCategoriesByName.FirstOrDefault();
                if (existCategoryByName != null) return Result<Guid>.Failure("Category already exists");
                existCategory.Name = request.Name.Trim();
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                existCategory.Description = request.Description.Trim();
            }

            var updatedCategory = _database.CategoryRepository.Update(existCategory, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(updatedCategory.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error updating category: {e.Message}");
        }
    }

    public async Task<Result<Guid>> Delete(BaseDeleteContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var existCategory = await _database.CategoryRepository.GetById(request.Id, ct);
            if (existCategory == null) return Result<Guid>.Failure("Category not found");
            
            var userByRequest = await _database.UserRepository.GetById(request.SenderId, ct);
            if (userByRequest == null) return Result<Guid>.Failure("User not found");
            if (!userByRequest.IsAdmin) return Result<Guid>.Failure("Action is not allowed for non admin users");
            
            var deletedCategory = await _database.CategoryRepository.Delete(request.Id, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(deletedCategory.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure($"Error deleting category: {e.Message}");
        }
    }

    public async Task<Result<List<CategoryDetailedDTO>>> GetAllCategoriesDetailed(CancellationToken ct)
    {
        try
        {
            var categories = await _database.CategoryRepository.GetAll(ct);
            var dtos = categories.Select(entity => new CategoryDetailedDTO(
                Id: entity.Id,
                Name: entity.Name,
                Description: entity.Description,
                CreatedAt: entity.CreatedAt,
                CreatorId: entity.CreatorId,
                Creator: new UserShortDTO(
                    Id: entity.CreatorId,
                    Username: entity.Creator.Username,
                    IsAdmin: entity.Creator.IsAdmin
                )
            )).ToList();
            return Result<List<CategoryDetailedDTO>>.Success(dtos);
        }
        catch (Exception e)
        {
            return Result<List<CategoryDetailedDTO>>.Failure($"Error getting categories: {e.Message}");
        }
    }

    public async Task<Result<List<CategoryShortDTO>>> GetAllCategoriesShort(CancellationToken ct)
    {
        try
        {
            var categories = await _database.CategoryRepository.GetAll(ct);
            var dtos = categories.Select(entity => new CategoryShortDTO(
                Id: entity.Id,
                Name: entity.Name,
                Description: entity.Description
            )).ToList();
            return Result<List<CategoryShortDTO>>.Success(dtos);
        }
        catch (Exception e)
        {
            return Result<List<CategoryShortDTO>>.Failure($"Error getting categories: {e.Message}");
        }
    }

    public async Task<Result<CategoryDetailedDTO>> GetCategoryById(Guid id, CancellationToken ct)
    {
        try
        {
            var category = await _database.CategoryRepository.GetById(id, ct);
            if (category == null) return Result<CategoryDetailedDTO>.Failure("Category not found");

            var dto = new CategoryDetailedDTO(
                Id: category.Id,
                Name: category.Name,
                Description: category.Description,
                CreatedAt: category.CreatedAt,
                CreatorId: category.CreatorId,
                Creator: new UserShortDTO(
                    Id: category.CreatorId,
                    Username: category.Creator.Username,
                    IsAdmin: category.Creator.IsAdmin
                )
            );
            return Result<CategoryDetailedDTO>.Success(dto);
        }
        catch (Exception e)
        {
            return Result<CategoryDetailedDTO>.Failure($"Error getting category: {e.Message}");
        }
    }
}