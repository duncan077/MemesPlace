using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MemesAPI.Data.Seeds
{
    public static partial class DataSeedExtension
    {
        public static void DataSeedEx(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(new
            {
                Id = "7f2443ad-1ee4-4133-858d-131e4137403e",
                Name = "Admin",
                NormalizedName = "ADMIN",

            },
            new
            {
                Id = "cf10ba0a-51dc-4c53-b6de-0f9f283fff74",
                Name = "User",
                NormalizedName = "USER",

            });
            var passwordHasher= new PasswordHasher<MemeUser>();
            modelBuilder.Entity<MemeUser>().HasData(
            
           new
           {
               Id = "043377e4-9f2c-42d3-9d02-88ea5adcfae7",
               FirstName = "Duncan",
               LastName = "Caceres Cartasso",
               Email="duncancacerescartasso@gmail.com",
               NormalizedEmail ="DUNCANCACERESCARTASSO@GMAIL.COM",
               UserName="duncan088",
               NormalizedUserName="DUNCAN088",
               PasswordHash = passwordHasher.HashPassword(null,"PasswordTest"),
               AccessFailedCount=0,
               EmailConfirmed=true,
               LockoutEnabled=false,
               PhoneNumberConfirmed=false,
               TwoFactorEnabled=false,
               Karma=1000,
               profilePic="",
               signature=""


           });
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new
           {
               RoleId= "7f2443ad-1ee4-4133-858d-131e4137403e",
               UserId= "043377e4-9f2c-42d3-9d02-88ea5adcfae7"

            });
            modelBuilder.Entity<Meme>().HasOne<MemeUser>(u => u.User)
                .WithMany(m => m.Memes).OnDelete( DeleteBehavior.Restrict);
        }
    }
   
}
