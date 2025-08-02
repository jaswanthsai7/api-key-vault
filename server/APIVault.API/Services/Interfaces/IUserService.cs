// File: Services/Interfaces/IUserService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIVault.API.Models;

namespace APIVault.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(Guid id, User user);
        Task<bool> DeleteUserAsync(Guid id);

        Task<bool> AssignRoleAsync(Guid userId, Guid roleId);
        Task<bool> AssignGroupAsync(Guid userId, Guid groupId);
    }
}
