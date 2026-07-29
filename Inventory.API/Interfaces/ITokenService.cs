using Inventory.API.Entities;

namespace Inventory.API.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}