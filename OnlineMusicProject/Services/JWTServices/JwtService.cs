using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineMusicProject.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OnlineMusicProject.Services.IServices;
using OnlineMusicProject.ViewModels;

namespace OnlineMusicProject.Services.JWTServices
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<Users> _userManager;
        private readonly OnlineMusicDBContext _context;

        public JwtService(IConfiguration configuration, UserManager<Users> userManager, OnlineMusicDBContext context)
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
        }

        public async Task<string> GenerateAccessToken(Users user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Gán role vào token
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(10),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<AuthResult> GenerateTokenJWT(Users user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // add roles
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                AddedDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddMinutes(2),
                IsRevoked = false,
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            var response = new AuthResult
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                ExpriseAt = token.ValidTo
            };

            return response;
        }

        public async Task<string> RefreshAccessToken(string refreshToken)
        {
            var principal = await ValidateToken(refreshToken);
            if (principal == null) return null;

            var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken);

            if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow || storedToken.IsRevoked)
            {
                return null;
            }

            var user = await _userManager.FindByIdAsync(storedToken.UserId);
            if (user == null) return null;

            return await GenerateAccessToken(user);
        }

        public async Task<ClaimsPrincipal> ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                }, out SecurityToken validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> RemoveRefreshTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
            if (token != null)
            {
                _context.RefreshTokens.Remove(token);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
