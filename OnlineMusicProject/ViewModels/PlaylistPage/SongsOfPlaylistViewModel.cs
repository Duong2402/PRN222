using OnlineMusicProject.Models;

namespace OnlineMusicProject.ViewModels.PlaylistPage
{
    public class SongsOfPlaylistViewModel
    {
        public Playlists playlistItem { get; set; }
        public PlaylistSongs SinglePlaylistSongs { get; set; }
        public List<PlaylistSongs> playlistSongs { get; set; }
    }
}
