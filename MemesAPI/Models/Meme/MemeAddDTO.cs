using MemesAPI.Data;
using Microsoft.Extensions.FileProviders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemesAPI.Models.Meme
{
    public class MemeAddDTO
    {
        [Required]
        [MaxLength(125)]
        [MinLength(5)]
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsFile { get; set; } = false;
        public string URLIMG { get; set; } = "";
        [Required]
        public bool IsVideo { get; set; }
        public List<string> Tags { get; set; } = new List<string>();

        public ImageFile Imgfile { get; set; } = new ImageFile();

    }
}
