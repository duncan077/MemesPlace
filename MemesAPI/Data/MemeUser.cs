using Microsoft.AspNetCore.Identity;

namespace MemesAPI.Data
{
    public class MemeUser : IdentityUser
    {
        public int Karma { get; set; }
        public virtual ICollection<Meme> Memes { get; set; }
    }
}
