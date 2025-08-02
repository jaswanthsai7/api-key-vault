// File: Services/Interfaces/IRoleService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIVault.API.Models;

namespace APIVault.API.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(Guid id);
        Task<Role> CreateRoleAsync(Role role);
        Task<Role> UpdateRoleAsync(Guid id, Role role);
        Task<bool> DeleteRoleAsync(Guid id);
    }
}
