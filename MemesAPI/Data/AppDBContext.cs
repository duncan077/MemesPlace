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
       public virtual DbSet<Meme> Memes { get; set; }
       public virtual DbSet<MemeLike> MemeLike { get; set; }
       public virtual DbSet<MemeUser> MemeUser { get; set; }
        public virtual DbSet<TagMeme> TagMeme { get; set; }
    }
}
