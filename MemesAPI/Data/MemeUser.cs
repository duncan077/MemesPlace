using Microsoft.AspNetCore.Identity;

namespace MemesAPI.Data
{
    public class MemeUser : IdentityUser
    {
        public string? signature { get; set; }
        public string? profilePic { get; set; }
        public int Karma { get; set; } = 0;
        public virtual ICollection<Meme>? Memes { get; set; }
    }
}
