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
        DbSet<Meme> Memes { get; set; }
        DbSet<MemeLike> MemeLike { get; set; }
        DbSet<MemeUser> MemeUser { get; set; }
    }
}
