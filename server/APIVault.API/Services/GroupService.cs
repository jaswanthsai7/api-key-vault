// File: Services/Implementations/GroupService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIVault.API.Models;
using APIVault.API.Services.Interfaces;
using APIVault.API.Data;

namespace APIVault.API.Services
{
    public class GroupService : IGroupService
    {
        private readonly AppDbContext _context;

        public GroupService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Group>> GetAllGroupsAsync()
        {
            return await _context.Groups
                .Include(g => g.Users)
                .Include(g => g.GroupApiScopes)
                .ToListAsync();
        }

        public async Task<Group> GetGroupByIdAsync(Guid id)
        {
            return await _context.Groups
                .Include(g => g.Users)
                .Include(g => g.GroupApiScopes)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Group> CreateGroupAsync(Group group)
        {
            group.Id = Guid.NewGuid();
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<Group> UpdateGroupAsync(Guid id, Group updatedGroup)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) return null;

            group.Name = updatedGroup.Name;
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<bool> DeleteGroupAsync(Guid id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) return false;

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
