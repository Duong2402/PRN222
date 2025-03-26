using System.ComponentModel.DataAnnotations;

namespace OnlineMusicProject.Models
{
    public class CreatePlaylistViewModel
    {
        [Required]
        [StringLength(255)]
        [Display(Name = "Playlist Name")]
        public string PlaylistName { get; set; }

        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; }

        [Display(Name = "Playlist Image (Optional)")]
        public IFormFile? PlaylistImage { get; set; }
    }
}