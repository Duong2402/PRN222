using OnlineMusicProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using OnlineMusicProject.Services;
using OnlineMusicProject.Services.IServices;
using OnlineMusicProject.Services.JWTServices;
using OnlineMusicProject.Services;
using System.Text;
using OnlineMusicProject.Services.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.MaxDepth = 64;
    });
builder.Services.AddDbContext<OnlineMusicDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineMusic"));
});
builder.Services.AddIdentity<Users, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
}).AddEntityFrameworkStores<OnlineMusicDBContext>().AddDefaultTokenProviders();
var emailConfig = configuration.GetSection("EmailConfiguration")
                                           .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
// configure Services
builder.Services.AddScoped<IMail, EmailService>();
builder.Services.AddScoped<IJwtService, JwtService>();
// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JWT");
var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
var app = builder.Build();
await SeedService.SeedDb(app.Services);
app.UseSession();
app.UseStaticFiles();
app.UseRouting();
app.UseMiddleware<JWTMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );
app.Run();




