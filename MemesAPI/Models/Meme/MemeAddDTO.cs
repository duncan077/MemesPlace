using MemesAPI.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemesAPI.Models.Meme
{
    public class MemeAddDTO
    {
       
        public string Name { get; set; }
        public string? Description { get; set; }
        public string URLIMG { get; set; }
    
    }
}
