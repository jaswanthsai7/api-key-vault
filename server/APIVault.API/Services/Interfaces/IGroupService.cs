// File: Services/Interfaces/IGroupService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIVault.API.Models;

namespace APIVault.API.Services.Interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<Group>> GetAllGroupsAsync();
        Task<Group> GetGroupByIdAsync(Guid id);
        Task<Group> CreateGroupAsync(Group group);
        Task<Group> UpdateGroupAsync(Guid id, Group group);
        Task<bool> DeleteGroupAsync(Guid id);
    }
}
