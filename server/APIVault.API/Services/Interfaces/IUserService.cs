// File: Services/Interfaces/IUserService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIVault.API.Models;

namespace APIVault.API.Services.Interfaces
{

    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid id);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(Guid id, User updatedUser);
        Task<bool> DeleteAsync(Guid id);
    }

}
