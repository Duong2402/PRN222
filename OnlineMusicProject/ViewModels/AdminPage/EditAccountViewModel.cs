using System.ComponentModel.DataAnnotations;

namespace OnlineMusicProject.ViewModels.AdminPage
{
    public class EditAccountViewModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "FullName can only contain letters and spaces.")]
        public string FullName { get; set; }
    }
}