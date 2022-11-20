
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MemesAPI.Models.User
{
    public class PasswordChange
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)] 
        public string VerifyPassword { get; set; } = string.Empty;

    }
}
