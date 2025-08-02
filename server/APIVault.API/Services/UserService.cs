// File: Services/Implementations/UserService.cs
using APIVault.API.Data;
using APIVault.API.Models;
using APIVault.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIVault.API.Services.Implementations
{

    using Microsoft.EntityFrameworkCore;
    using APIVault.API.Models;

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Group)
                    .ThenInclude(g => g.GroupApiScopes)
                        .ThenInclude(gas => gas.ApiScope)
                .Include(u => u.ApiKeys)
                    .ThenInclude(k => k.ApiKeyScopes)
                        .ThenInclude(ks => ks.ApiScope)
                .ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Group)
                    .ThenInclude(g => g.GroupApiScopes)
                        .ThenInclude(gas => gas.ApiScope)
                .Include(u => u.ApiKeys)
                    .ThenInclude(k => k.ApiKeyScopes)
                        .ThenInclude(ks => ks.ApiScope)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> CreateAsync(User user)
        {
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(Guid id, User updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            user.Email = updatedUser.Email;
            user.PasswordHash = updatedUser.PasswordHash;
            user.RoleId = updatedUser.RoleId;
            user.GroupId = updatedUser.GroupId;

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }



}
