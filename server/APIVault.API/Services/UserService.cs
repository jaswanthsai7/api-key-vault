// File: Services/Implementations/UserService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIVault.API.Models;
using APIVault.API.Services.Interfaces;
using APIVault.API.Data;

namespace APIVault.API.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Group)
                .Include(u => u.ApiKeys)
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Group)
                .Include(u => u.ApiKeys)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(Guid id, User updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            user.Email = updatedUser.Email;
            user.PasswordHash = updatedUser.PasswordHash;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignRoleAsync(Guid userId, Guid roleId)
        {
            var user = await _context.Users.FindAsync(userId);
            var role = await _context.Roles.FindAsync(roleId);
            if (user == null || role == null) return false;

            user.RoleId = roleId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignGroupAsync(Guid userId, Guid groupId)
        {
            var user = await _context.Users.FindAsync(userId);
            var group = await _context.Groups.FindAsync(groupId);
            if (user == null || group == null) return false;

            user.GroupId = groupId;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
