using APIVault.API.Data;
using APIVault.API.DTOs.Auth;
using APIVault.API.Helpers;
using APIVault.API.Models;
using APIVault.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIVault.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtHelper _jwtHelper;

        public AuthService(AppDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new Exception("Invalid email or password.");

            return _jwtHelper.GenerateAccessToken(user);
        }

        public async Task<User> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
                throw new Exception("User already exists with this email.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var role = await _context.Roles.FindAsync(request.RoleId);
            var group = await _context.Groups.FindAsync(request.GroupId);
            if (role == null || group == null)
                throw new Exception("Invalid Role or Group ID.");

            var newUser = new User
            {
                Email = request.Email,
                PasswordHash = hashedPassword,
                RoleId = role.Id,
                GroupId = group.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }
    }
}
