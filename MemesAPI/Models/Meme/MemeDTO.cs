namespace MemesAPI.Models.Meme
{
    public class MemeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string imgProfile { get; set; } = "";
       public string UserName { get; set; } = "";
        public DateTime Date { get; set; }
        public string URLIMG { get; set; } = "";
        public bool IsVideo { get; set; }
        public string Format { get; set; } = "";
        public List<string> Tags { get; set; } = new List<string>();
        public int likeCount { get; set; } =0;
        public bool like { get; set; } = false;
    }
}
