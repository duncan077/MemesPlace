using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MemesAPI.Data
{
    public partial class AppDBContext : IdentityDbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        protected AppDBContext()
        {
        }
       public DbSet<Meme> Memes { get; set; }
       public DbSet<MemeLike> MemeLike { get; set; }
       public DbSet<MemeUser> MemeUser { get; set; }
    }
}
