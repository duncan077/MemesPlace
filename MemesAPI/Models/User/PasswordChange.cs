
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MemesAPI.Models.User
{
    public class PasswordChange
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword= string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string VerifyPassword = string.Empty;

    }
}
