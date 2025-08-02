// File: Controllers/UserController.cs
using APIVault.API.Data;
using APIVault.API.DTOs.User;
using APIVault.API.Models;
using APIVault.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace APIVault.API.Controllers.Admin
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        // GET: /api/user
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        // GET: /api/user/{id}
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _service.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        // POST: /api/user
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = dto.PasswordHash,
                RoleId = dto.RoleId,
                GroupId = dto.GroupId,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _service.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: /api/user/{id}
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto dto)
        {
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = dto.PasswordHash,
                RoleId = dto.RoleId,
                GroupId = dto.GroupId
            };

            var updated = await _service.UpdateAsync(id, user);
            return updated == null ? NotFound() : Ok(updated);
        }

        // DELETE: /api/user/{id}
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }


}
