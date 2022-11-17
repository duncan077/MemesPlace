using MemesAPI.Data.Seeds;
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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            DataSeedExtension.DataSeedEx(builder);
            base.OnModelCreating(builder);
        }
        public virtual DbSet<Meme> Memes { get; set; }
       public virtual DbSet<MemeLike> MemeLike { get; set; }
       public virtual DbSet<MemeUser> MemeUser { get; set; }
        public virtual DbSet<TagMeme> TagMeme { get; set; }
    }
}
