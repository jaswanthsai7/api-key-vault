using APIVault.API.DTOs.Admin;
using APIVault.API.Models;

namespace APIVault.API.Services.Interfaces
{
    public interface IAdminService
    {
        Task<Role> CreateRoleAsync(CreateRoleRequest request);
        Task<IEnumerable<Role>> GetAllRolesAsync();
    }
}
