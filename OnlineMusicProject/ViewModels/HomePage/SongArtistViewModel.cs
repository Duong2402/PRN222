using OnlineMusicProject.Models;

namespace OnlineMusicProject.ViewModels.HomePage
{
    public class SongArtistViewModel
    {
        public List<Songs> Songs { get; set; }
        public List<Artists> Artists { get; set; }
        public Artists ArtistOfSongs { get; set; }
        public List<SongGenres> Genres { get; set; }
    }
}
