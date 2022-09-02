using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MemesAPI.Data
{
    public class Meme
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string URLIMG { get; set; }
        
       
        public DateTime Date { get; set; }
        [ForeignKey(nameof(MemeUser))]
        public  List<MemeUser>? Likes { get; set; }
        [ForeignKey(nameof(MemeUser.UserName))]
        public virtual string UserName { get; set; }
    }
}
