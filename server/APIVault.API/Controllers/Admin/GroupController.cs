// File: Controllers/GroupController.cs
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APIVault.API.Models;
using APIVault.API.Services.Interfaces;

namespace APIVault.API.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var groups = await _groupService.GetAllGroupsAsync();
            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var group = await _groupService.GetGroupByIdAsync(id);
            if (group == null) return NotFound();
            return Ok(group);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Group group)
        {
            var created = await _groupService.CreateGroupAsync(group);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Group group)
        {
            var updated = await _groupService.UpdateGroupAsync(id, group);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _groupService.DeleteGroupAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
