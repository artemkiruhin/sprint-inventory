using System.Security.Cryptography;
using System.Text;
using SprintInventory.Core.Interfaces.Services.Security;

namespace SprintInventory.Services.Security;

public class Sha256Hasher : IHashService
{
    public string HashData(string data)
    {
        return Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(data)));
    }
}