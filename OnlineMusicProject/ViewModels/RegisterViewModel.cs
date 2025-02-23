using System.ComponentModel.DataAnnotations;

namespace OnlineMusicProject.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Full Name")]
        [StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "FullName can only contain letters and spaces.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "The password must have at least 6 characters.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage ="Password does not match.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
