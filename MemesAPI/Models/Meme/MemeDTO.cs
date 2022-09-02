namespace MemesAPI.Models.Meme
{
    public class MemeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
       
        public string imgURL { get; set; }
        public bool like { get; set; }
    }
}
