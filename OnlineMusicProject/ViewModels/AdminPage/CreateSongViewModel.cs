using System.ComponentModel.DataAnnotations;

namespace OnlineMusicProject.ViewModels.AdminPage
{
    public class CreateSongViewModel
    {
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

        [Display(Name = "Song Image (Optional)")]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Please upload an audio file.")]
        [Display(Name = "Audio File (Required)")]
        public IFormFile AudioFile { get; set; }
    }
}