using OnlineMusicProject.Models;
using OnlineMusicProject.ViewModels.SongPage;

namespace OnlineMusicProject.ViewModels.HomePage
{
    public class PlaylistViewModel
    {
        public List<Playlists> PlaylistItems { get; set; }
        public List<playlistWithCounts> PlaylistItemsWithCounts { get; set; }
    }
}
