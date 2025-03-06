using System.ComponentModel.DataAnnotations;

namespace OnlineMusicProject.Models
{
    public class SongGenres
    {
        [Key]
        public Guid GenreId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Songs> Songs { get; set; }

    }
}
