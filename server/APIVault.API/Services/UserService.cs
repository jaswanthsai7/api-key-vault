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

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Group)
                    .ThenInclude(g => g.GroupApiScopes)
                        .ThenInclude(gs => gs.ApiScope)
                .Include(u => u.ApiKeys)
                    .ThenInclude(k => k.ApiKeyScopes)
                        .ThenInclude(s => s.ApiScope)
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Group)
                    .ThenInclude(g => g.GroupApiScopes)
                        .ThenInclude(gs => gs.ApiScope)
                .Include(u => u.ApiKeys)
                    .ThenInclude(k => k.ApiKeyScopes)
                        .ThenInclude(s => s.ApiScope)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                PasswordHash = dto.PasswordHash,
                RoleId = dto.RoleId,
                GroupId = dto.GroupId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Email = dto.Email;
            user.PasswordHash = dto.PasswordHash;
            user.RoleId = dto.RoleId;
            user.GroupId = dto.GroupId;

            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }


}
