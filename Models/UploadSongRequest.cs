namespace MusicApp.Models
{
    public class UploadSongRequest
    {
        public string Title { get; set; } = string.Empty;

        public IFormFile File { get; set; } = null!;
    }
}