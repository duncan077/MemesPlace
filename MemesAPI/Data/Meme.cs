namespace MemesAPI.Data
{
    public class Meme
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string URLIMG { get; set; }
        
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<MemeLike> Likes { get; set; }
        public virtual MemeUser User { get; set; }
    }
}
