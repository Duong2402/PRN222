using OnlineMusicProject.Models;

namespace OnlineMusicProject.ViewModels.HomePage
{
    public class PlaylistsViewModel
    {
        public Playlists SinglePlaylist { get; set; }
        public List<Playlists> PlaylistList { get; set; }
    }
}
