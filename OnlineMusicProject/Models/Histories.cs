using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMusicProject.Models
{
    public class Histories
    {
        [Key]
        public Guid HistoryId {  get; set; }
        [ForeignKey(nameof(Users))]  
        public string UserId { get; set; }
        [ForeignKey(nameof(Songs))]
        public Guid? SongId { get; set; }
        public Guid? AlbumId { get; set; }
        public DateTime? PlayedAt { get; set; }
        public Users User { get; set; }
        public Songs Songs { get; set; }
        public Albums Albums { get; set; }
    }
}
