using OnlineMusicProject.Models;
using OnlineMusicProject.ViewModels.SongPage;

namespace OnlineMusicProject.ViewModels.PlaylistPage
{
    public class SongsOfPlaylistViewModel
    {
        public string UserName { get; set; }
        public List<Playlists> PlaylistItems { get; set; }
        public List<playlistWithCounts> PlaylistItemsWithCounts { get; set; }
        public Playlists playlistItem { get; set; }
        public PlaylistSongs SinglePlaylistSongs { get; set; }
        public List<PlaylistSongs> playlistSongs { get; set; }
        public Guid? CurrentSongId { get; set; }
    }
}
