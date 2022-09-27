using Microsoft.AspNetCore.Identity;

namespace MemesAPI.Data
{
    public class MemeUser : IdentityUser
    {
        public MemeUser()
        {
            Memes = new HashSet<Meme>();
            Likes = new HashSet<MemeLike>();
        }

        public string signature { get; set; } = "";
        public string profilePic { get; set; } = "";
        public int Karma { get; set; } = 0;
        public virtual ICollection<Meme>? Memes { get; set; }
        public virtual ICollection<MemeLike> Likes { get; set; }
    }
}
