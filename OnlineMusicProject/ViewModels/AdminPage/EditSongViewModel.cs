using System.ComponentModel.DataAnnotations;

namespace OnlineMusicProject.ViewModels.AdminPage
{
    public class EditSongViewModel
    {
        public Guid SongId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Name Song")]
        public string NameSong { get; set; }

        [Required]
        [Display(Name = "Artist")]
        public Guid ArtistId { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public Guid GenreId { get; set; }

        public string? Lyrics { get; set; }

        [Display(Name = "Current Image")]
        public string? ImageSong { get; set; }

        [Display(Name = "Song Image (Optional)")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Current Audio")]
        public string? FilePath { get; set; }

        [Display(Name = "Audio File (Optional)")]
        public IFormFile? AudioFile { get; set; }
    }
}