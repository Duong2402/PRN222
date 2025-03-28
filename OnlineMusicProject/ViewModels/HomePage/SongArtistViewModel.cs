using OnlineMusicProject.Models;

namespace OnlineMusicProject.ViewModels.HomePage
{
    public class SongArtistViewModel
    {
        public List<Songs> SongsWeekTop { get; set; }
        public List<Songs> SongsNewHit { get; set; }
        public List<Songs> Songs { get; set; }
        public List<Artists> Artists { get; set; }
        public Songs MaxListener { get; set; }
        public Artists ArtistOfSongs { get; set; }
        public List<SongGenres> Genres { get; set; }
        public List<Albums> Albums { get; set; }
    }
}
