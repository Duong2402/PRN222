using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace OnlineMusicProject.Models
{
    public class Users: IdentityUser
    {
        [Display(Name = "Full Name")]
        [StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "FullName can only contain letters and spaces.")]
        public string FullName { get; set; }

    }
}
