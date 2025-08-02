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
        Task<User> UpdateUserAsync(Guid id, User updatedUser);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
