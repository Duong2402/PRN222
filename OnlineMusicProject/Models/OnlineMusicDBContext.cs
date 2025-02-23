using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OnlineMusicProject.Models
{
    public class OnlineMusicDBContext: IdentityDbContext<Users>
    {
        public OnlineMusicDBContext(DbContextOptions options) : base(options)
        {
        }
    }
}
