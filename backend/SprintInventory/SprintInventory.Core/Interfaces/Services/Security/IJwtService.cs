using SprintInventory.Core.Models.DTOs;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Core.Interfaces.Services.Security;

public interface IJwtService
{
    Result<string> GenerateToken(Guid userId);
}