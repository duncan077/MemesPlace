using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MemesAPI.Data
{
    [Keyless]
    public class MemeLike
    {
        
        [ForeignKey(nameof(MemeUser))]
        public string UserName { get; set; }
        public DateTime DateTime { get; set; }
        [ForeignKey(nameof(Meme))]
        public int Meme { get; set; }
    }
}
