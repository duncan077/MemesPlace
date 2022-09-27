using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemesAPI.Data
{
    public class TagMeme
    {
        public TagMeme()
        {
            Memes= new HashSet<Meme>();
        }
        [Required]
        public string Name { get; set; } = "";
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get;set; }
        public virtual ICollection<Meme> Memes { get; set; }
    }
}
