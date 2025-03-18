using OnlineMusicProject.Models;

namespace OnlineMusicProject.ViewModels.SongPage
{
    public class SongPlaylistViewModel
    {
        public Songs Song { get; set; }
        public List<Songs> Songs { get; set; }
		public List<Playlists> PlaylistItems { get; set; }
        public List<playlistWithCounts> PlaylistItemsWithCounts { get; set; }
    }
}
