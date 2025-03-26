using OnlineMusicProject.ViewModels;
using OnlineMusicProject.Models;
using System.Security.Claims;

namespace OnlineMusicProject.Services.IServices
{
    public interface IJwtService
    {
        public Task<AuthResult> GenerateTokenJWT(Users user);
        public Task<string> GenerateAccessToken(Users user);
        public Task<string> RefreshAccessToken(string refreshToken);
        public Task<ClaimsPrincipal> ValidateToken(string token);
        Task<bool> RemoveRefreshTokenAsync(string refreshToken);
    }
}
