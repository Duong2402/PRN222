﻿using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMusicProject.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }

        public bool IsRevoked { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        [ForeignKey("UserId")]
        public Users User { get; set; }
    }
}
