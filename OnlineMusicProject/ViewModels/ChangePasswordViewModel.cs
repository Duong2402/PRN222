using System.ComponentModel.DataAnnotations;

namespace OnlineMusicProject.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string Token { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "The password must have at least 6 characters.")]
        [Display(Name ="New Password")]
        [DataType(DataType.Password)]
        [Compare("ConfirmNewPassword", ErrorMessage = "Password does not match.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Display(Name = "Confirm New Password")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
