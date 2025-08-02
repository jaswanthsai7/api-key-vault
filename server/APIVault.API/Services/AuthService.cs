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
        private readonly EncryptionHelper _encryptionHelper;

        public AuthService(AppDbContext context, JwtHelper jwtHelper, EncryptionHelper encryptionHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _encryptionHelper = encryptionHelper;
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                throw new Exception("Invalid email or password.");

            // Decrypt the encrypted password received from frontend
            string decryptedPassword = _encryptionHelper.Decrypt(request.Password);

            // Verify decrypted password against stored hash
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(decryptedPassword, user.PasswordHash);

            if (!isPasswordValid)
                throw new Exception("Invalid email or password.");

            return _jwtHelper.GenerateAccessToken(user);
        }


        public async Task<User> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
                throw new Exception("User already exists with this email.");

            // Decrypt password first
            string decryptedPassword = _encryptionHelper.Decrypt(request.Password);

            // Hash decrypted password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(decryptedPassword);

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
