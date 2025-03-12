using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMusicProject.Models
{
    public class Songs
    {
        [Key]
        public Guid SongId { get; set; }

        [Display(Name = "Image Song")]
        [StringLength(255)]
        public string? ImageSong { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Name Song")]
        public string NameSong { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "File Path")]
        public string FilePath { get; set; }
        public string Lyrics { get; set; }
        [Display(Name = "Number Of Listeners")]
        public int NumberOfListeners { get; set; }
        public Guid GenreId { get; set; }
        public SongGenres songGenres { get; set; }
        public Guid ArtistId {  get; set; }
        public Artists Artists { get; set; }
        public ICollection<PlaylistSongs> PlaylistSongs { get; set; }
        public ICollection<Histories> Histories { get; set; }

    }
}
