using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OnlineMusicProject.Models
{
    public class OnlineMusicDBContext: IdentityDbContext<Users>
    {
        public OnlineMusicDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Artists> Artists { get; set; }

        public DbSet<Histories> Histories { get; set; }

        public DbSet<Playlists> Playlists { get; set; }

        public DbSet<PlaylistSongs> PlaylistSongs { get; set; }

        public DbSet<SongGenres> SongGenres { get; set; }
        public DbSet<Songs> Songs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PlaylistSongs>()
                .HasKey(ps => new { ps.PlaylistId, ps.SongId });

            modelBuilder.Entity<Histories>()
       .HasOne(h => h.User)
       .WithMany()
       .HasForeignKey(h => h.UserId)
       .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Playlists>()
         .HasOne(p => p.User)
         .WithMany()
         .HasForeignKey(p => p.UserId)
         .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Songs>()
           .HasOne(s => s.Artists) 
           .WithMany(a => a.Songs)
           .HasForeignKey(s => s.ArtistId) 
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Songs>()
          .HasOne(s => s.songGenres)
          .WithMany(a => a.Songs)
          .HasForeignKey(s => s.GenreId)
          .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlaylistSongs>()
       .HasOne(ps => ps.Playlists)    
       .WithMany(p => p.PlaylistSongs) 
       .HasForeignKey(ps => ps.PlaylistId) 
       .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlaylistSongs>()
                .HasOne(ps => ps.Songs)    
                .WithMany(s => s.PlaylistSongs)  
                .HasForeignKey(ps => ps.SongId) 
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
