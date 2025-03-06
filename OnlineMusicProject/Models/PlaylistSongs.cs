using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMusicProject.Models
{
    public class PlaylistSongs
    {
        public Guid PlaylistId {  get; set; }
        public Guid SongId {  get; set; }   
        public DateTime? AddAt {  get; set; } = DateTime.Now;

        [Display(Name = "Play_Count")]
        public int? PlayCount { get; set; }
        public Playlists Playlists { get; set; }
        public Songs Songs { get; set; }
    }
}
