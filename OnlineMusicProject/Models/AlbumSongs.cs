namespace OnlineMusicProject.Models
{
    public class AlbumSongs
    {
        public Guid AlbumId { get; set; }
        public Guid? SongId { get; set; }
        public Albums Albums { get; set; }
        public Songs Songs { get; set; }
    }
}
