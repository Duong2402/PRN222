using Microsoft.AspNetCore.Identity;


namespace OnlineMusicProject.Models
{
    public class Users: IdentityUser
    {
        public string FullName { get; set; }

    }
}
