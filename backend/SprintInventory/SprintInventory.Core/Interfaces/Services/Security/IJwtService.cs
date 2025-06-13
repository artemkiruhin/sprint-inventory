using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Core.Interfaces.Services.Security;

public interface IJwtService
{
    string GenerateToken(Guid userId);
}