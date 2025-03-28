using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineMusicProject.Models
{
    public class Playlists
    {
        [Key]
        public Guid PlaylistId { get; set; }
        [Required]
        [Display(Name = "Playlist Name")]
        [StringLength(255)]
        public string PlaylistName { get; set; }
        [Display(Name = "Playlist Image")]
        public string? PlaylistImage { get; set; }

        [ForeignKey(nameof(Users))]
        public string UserId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public bool IsPublic { get; set; }
        public Users User { get; set; }
        public ICollection<PlaylistSongs> PlaylistSongs { get; set; }
    }
}
