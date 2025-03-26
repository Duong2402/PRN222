using System.ComponentModel.DataAnnotations;

namespace OnlineMusicProject.ViewModels.AdminPage
{
    public class CreateAccountViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "FullName can only contain letters and spaces.")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public string? Role { get; set; }
    }
}