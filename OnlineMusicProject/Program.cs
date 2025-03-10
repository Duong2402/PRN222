
using OnlineMusicProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using OnlineMusicProject.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
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
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
var app = builder.Build();
await SeedService.SeedDb(app.Services);
app.UseSession();
app.UseStaticFiles();
app.UseRouting(); ;
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );
app.Run();




