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
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class LoginUserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
