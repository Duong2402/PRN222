using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OnlineMusicProject.Services.IServices;

namespace OnlineMusicProject.Services.JWT
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;

        public JWTMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJwtService jwtService)
        {
            var token = context.Request.Cookies["access_Token"];
            ClaimsPrincipal principal = null;

            if (!string.IsNullOrEmpty(token))
            {
                principal = await jwtService.ValidateToken(token);
            }

            if (principal == null) // Token hết hạn hoặc không hợp lệ
            {
                var refreshToken = context.Request.Cookies["refresh_Token"];
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    var newTokens = await jwtService.RefreshAccessToken(refreshToken);
                    if (newTokens != null)
                    {
                        context.Response.Cookies.Append("access_Token", newTokens, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = false, // Nên để true nếu dùng HTTPS
                            SameSite = SameSiteMode.None
                        });

                        principal = await jwtService.ValidateToken(newTokens);
                    }
                    else
                    {
                        await jwtService.RemoveRefreshTokenAsync(refreshToken);
                        context.Response.Cookies.Delete("access_Token");
                        context.Response.Cookies.Delete("refresh_Token");
                        context.User = new ClaimsPrincipal(new ClaimsIdentity());
                        context.Response.Redirect("/Account/Login");
                        return;
                    }
                }
            }

            if (principal != null)
            {
                context.User = principal;
            }

            await _next(context);
        }
    }
}
