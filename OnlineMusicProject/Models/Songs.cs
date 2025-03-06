using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMusicProject.Models
{
    public class Songs
    {
        [Key]
        public Guid SongId { get; set; }

        [Display(Name = "Image_Song")]
        [StringLength(255)]
        public string? ImageSong { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Name_Song")]
        public string NameSong { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "File_Path")]
        public string FilePath { get; set; }
        [ForeignKey("GenreId")]

        public Guid GenreId { get; set; }
        [ForeignKey("ArtistId")]
        public Guid ArtistId {  get; set; }
        public Artists Artists { get; set; }
        public SongGenres songGenres { get; set; }
        public ICollection<PlaylistSongs> PlaylistSongs { get; set; }
        public ICollection<Histories> Histories { get; set; }

    }
}
