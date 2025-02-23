using System.ComponentModel.DataAnnotations;

namespace OnlineMusicProject.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid email.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name ="Remember me?")]
        public bool RememberMe { get; set; }
    }
}
