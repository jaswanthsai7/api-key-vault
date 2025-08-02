// File: Controllers/UserController.cs
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APIVault.API.Models;
using APIVault.API.Services.Interfaces;

namespace APIVault.API.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var created = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, User user)
        {
            var updated = await _userService.UpdateUserAsync(id, user);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpPost("{id}/assign-role/{roleId}")]
        public async Task<IActionResult> AssignRole(Guid id, Guid roleId)
        {
            var result = await _userService.AssignRoleAsync(id, roleId);
            return result ? Ok() : NotFound();
        }

        [HttpPost("{id}/assign-group/{groupId}")]
        public async Task<IActionResult> AssignGroup(Guid id, Guid groupId)
        {
            var result = await _userService.AssignGroupAsync(id, groupId);
            return result ? Ok() : NotFound();
        }
    }
}
