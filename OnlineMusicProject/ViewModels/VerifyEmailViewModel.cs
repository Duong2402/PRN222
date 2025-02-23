using System.ComponentModel.DataAnnotations;

namespace OnlineMusicProject.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email.")]
        public string Email { get; set; }
    }
}
