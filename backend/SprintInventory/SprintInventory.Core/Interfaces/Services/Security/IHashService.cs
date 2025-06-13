namespace SprintInventory.Core.Interfaces.Services.Security;

public interface IHashService
{
    string HashData(string data);
}