namespace MusicApp.Models
{
    public class Song
    {

        public int Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string Artist { get; set; } = string.Empty;

        public string ContentType { get; set; } = "video/mp4";
        
        public string Title { get; set; } = string.Empty;
     
        public DateTime CreatedAt { get; set; }
    }
}
