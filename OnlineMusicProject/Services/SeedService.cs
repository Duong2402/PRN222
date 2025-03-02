using Microsoft.AspNetCore.Identity;
using OnlineMusicProject.Models;

namespace OnlineMusicProject.Services
{
    public class SeedService
    {
        public static async Task SeedDb(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OnlineMusicDBContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedService>>();
            try
            {
                logger.LogInformation("Ensuring the database is created.");
                await context.Database.EnsureCreatedAsync();

                logger.LogInformation("roles");
                await AddRoleAsync(roleManager, "Admin");
                await AddRoleAsync(roleManager, "User");

                logger.LogInformation("Seeding admin user.");
                var adminEmail = "admin@gmail.com";
                if(await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var adminUser = new Users
                    {
                        FullName = "Adminne",
                        UserName = adminEmail,
                        NormalizedUserName = adminEmail.ToUpper(),
                        Email = adminEmail,
                        NormalizedEmail = adminEmail.ToUpper(),
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString(),
                    };
                    var result = await userManager.CreateAsync(adminUser, "Admin123");
                    if (result.Succeeded)
                    {
                        logger.LogInformation("Assigning Admin role");
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    else
                    {
                        logger.LogError("Failed to create admin: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                   
                }
            }catch (Exception ex)
            {
                logger.LogError(ex, "An errors occurred while seeding the database");
            }
        }
        private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if(!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
