using System.ComponentModel.DataAnnotations;

namespace OnlineMusicProject.Models
{
    public class Artists
    {
        [Key]
        public Guid ArtistId {  get; set; }

        [Required]
        [Display(Name = "Artist_Image")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        [StringLength(255)]
        public string? ArtistImage {  get; set; }

        [Required]
        [Display(Name = "Artist_Name")]
        [StringLength(255, ErrorMessage = "Artist name cannot be longer than 255 characters.")]
        public string ArtistName { get; set; }

        public string? Bio { get; set; }
        public ICollection<Songs> Songs { get; set; }


    }
}
