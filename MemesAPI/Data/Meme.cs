using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MemesAPI.Data
{
    public class Meme
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(60)]
        [MinLength(5)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }
        [Required]
        [Url]
        public string URLIMG { get; set; }
        [Url]
        public string? profilePic { get; set; }

        public DateTime Date { get; set; }
        

        public HashSet<MemeLike>? Likes { get; set; } = new();
        [Required]
        [ForeignKey(nameof(MemeUser.UserName))]
        public virtual string UserName { get; set; }
    }
}
