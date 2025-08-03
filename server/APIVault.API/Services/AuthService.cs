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

            // Decrypt password from frontend (AES or similar)
            string decryptedPassword = _encryptionHelper.Decrypt(request.Password);

            // Verify against hashed password in DB (BCrypt)
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(decryptedPassword, user.PasswordHash);

            if (!isPasswordValid)
                throw new Exception("Invalid email or password.");

            // Generate JWT if valid
            return _jwtHelper.GenerateAccessToken(user);
        }



        public async Task<User> RegisterAsync(RegisterRequest request)
        {
            // Check if email already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
                throw new Exception("User already exists with this email.");

            // Decrypt password (if required by your system)
            string decryptedPassword = _encryptionHelper.Decrypt(request.Password);

            // Hash the decrypted password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(decryptedPassword);

            // ✅ Assign default Role and Group
            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Unassigned");
            var defaultGroup = await _context.Groups.FirstOrDefaultAsync(g => g.Name == "Default");

            if (defaultRole == null || defaultGroup == null)
                throw new Exception("Default Role or Group not found. Please contact administrator.");

            // Create new user with default RoleId and GroupId
            var newUser = new User
            {
                Email = request.Email,
                PasswordHash = hashedPassword,
                RoleId = defaultRole.Id,
                GroupId = defaultGroup.Id,
                CreatedAt = DateTime.UtcNow
            };

            // Save user
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

    }
}
