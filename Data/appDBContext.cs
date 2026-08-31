using Microsoft.EntityFrameworkCore;
using MusicApp.Models;

namespace MusicApp.Data

{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }
            public DbSet<Song> Songs { get; set; }

        
    }
}
    
    