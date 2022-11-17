using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MemesAPI.Data
{
    
    public class MemeLike
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey(nameof(MemeUser.UserName))]
        public string UserName { get; set; }
        public DateTime DateTime { get; set; }
        [ForeignKey("MemeD")]
        public int MemeId { get; set; }

        public virtual Meme MemeD { get; set; }
    }
}
