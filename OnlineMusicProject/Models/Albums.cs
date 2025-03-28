using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMusicProject.Models
{
    public class Albums
    {
        [Key]
        public Guid AlbumId {  get; set; }
        public string Album_Image { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid ArtistId {  get; set; }
        public Artists Artists { get; set; }
        [ForeignKey(nameof(Users))]
        public string? UserId { get; set; }
        public Users User { get; set; }
        public ICollection<Histories> Histories { get; set; }
        public ICollection<AlbumSongs> AlbumSongs { get; set; }
    }
}
