using OnlineMusicProject.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineMusicProject.ViewModels.PlaylistPage
{
    public class SongsViewModel
    {
        public string NameSong { get; set; }
        public IFormFile FilePath { get; set; }
        public Guid GenreId { get; set; }
        public Guid ArtistId { get; set; }
        public string? UserId { get; set; }
        public bool? IsPublic { get; set; }
    }
}
