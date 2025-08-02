using APIVault.API.Data;
using APIVault.API.DTOs.Admin;
using APIVault.API.Models;
using APIVault.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIVault.API.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Role> CreateRoleAsync(CreateRoleRequest request)
        {
            if (await _context.Roles.AnyAsync(r => r.Name == request.Name))
                throw new Exception("Role already exists.");

            var role = new Role { Name = request.Name };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }
    }
}
