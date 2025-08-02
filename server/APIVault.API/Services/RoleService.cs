// File: Services/Implementations/RoleService.cs
using Microsoft.EntityFrameworkCore;
using APIVault.API.Models;
using APIVault.API.Services.Interfaces;
using APIVault.API.Data;

namespace APIVault.API.Services
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;

        public RoleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(Guid id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<Role> CreateRoleAsync(Role role)
        {
            role.Id = Guid.NewGuid();
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Role> UpdateRoleAsync(Guid id, Role updatedRole)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return null;

            role.Name = updatedRole.Name;
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
