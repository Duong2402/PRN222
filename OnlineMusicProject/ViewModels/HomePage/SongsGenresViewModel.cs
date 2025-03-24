using OnlineMusicProject.Models;
using OnlineMusicProject.Services.Pagination;

namespace OnlineMusicProject.ViewModels.HomePage
{
    public class SongsGenresViewModel
    {
        public Pagination<Songs> Songs { get; set; }
        public SongGenres SongGenres { get; set; }
    }
}
