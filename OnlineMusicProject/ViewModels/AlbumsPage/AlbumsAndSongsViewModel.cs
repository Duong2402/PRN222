using OnlineMusicProject.Models;
using OnlineMusicProject.ViewModels.SongPage;

namespace OnlineMusicProject.ViewModels.AlbumsPage
{
	public class AlbumsAndSongsViewModel
	{
		public Albums album {  get; set; }	
		public List<AlbumSongs> albumSongs { get; set; }
		public AlbumSongs SingleAlbumSongs { get; set; }
		public List<Playlists> PlaylistItems { get; set; }
		public List<playlistWithCounts> PlaylistItemsWithCounts { get; set; }
	}
}
