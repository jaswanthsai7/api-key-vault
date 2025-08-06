using APIVault.API.DTOs.Auth;
using APIVault.API.Models;

namespace APIVault.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterRequest request);
        Task<(User user, string role, string token)> LoginAsync(LoginRequest request); 
    }
}
