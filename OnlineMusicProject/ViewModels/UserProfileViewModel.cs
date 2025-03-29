using OnlineMusicProject.Models;

namespace OnlineMusicProject.ViewModels
{
    public class UserProfileViewModel
    {
        public Users User { get; set; }
        public List<Histories> histories { get; set; }
    }
}
