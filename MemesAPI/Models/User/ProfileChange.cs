
using MemesAPI.Models.Meme;
using System.ComponentModel.DataAnnotations;

namespace MemesAPI.Models.User
{
    public class ProfileChange
    {
        [Required]
        public string Password { get; set; } =string.Empty;
        [Required]
        public string Signature { get; set; } = string.Empty;
      

        public string profilePic { get; set; } = string.Empty;
        public ImageFile image { get; set; }
        [Required]
        public bool IsFile { get; set; } = false;
    }
}
