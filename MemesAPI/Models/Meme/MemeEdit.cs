using System.ComponentModel.DataAnnotations;

namespace MemesAPI.Models.Meme
{
    public class MemeEditDTO
    {
        [Required]
        public string Name {get;set;}
        public string Description { get; set; } = "";

        public List<string> Tags { get; set; } = new List<string>();
    }
}
