using System.ComponentModel.DataAnnotations;

namespace OnlineMusicProject.ViewModels.AdminPage
{
    public class EditPlaylistViewModel
    {
        public Guid PlaylistId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Playlist Name")]
        public string PlaylistName { get; set; }

        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; }

        [Display(Name = "Current Image")]
        public string? PlaylistImage { get; set; }

        [Display(Name = "Playlist Image (Optional)")]
        public IFormFile? PlaylistImageFile { get; set; }
    }
}