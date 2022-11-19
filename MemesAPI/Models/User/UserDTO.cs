using MemesAPI.Models.Meme;
using System.ComponentModel.DataAnnotations;

namespace MemesAPI.Models.User
{
    public class UserDTO
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
 
    }
    public class LoginUserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public record ProfileDTO(string UserName, int Karma, string profilePic,string signature,ICollection<MemeDTO> LastMemes);
}
