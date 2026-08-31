using MusicApp.Data;
using MusicApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicApp.Services
{
    public class SongsService
    {
        private readonly AppDBContext _context;

        public SongsService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<Song>> GetSongAsync()
        {
            var songs = await _context.Songs
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return songs;
        }

        public async Task<Song?> GetSongByIdAsync(int id)
        {
            var song = await _context.Songs.FindAsync(id);

            return song;
        }

        public async Task<Song> UploadSongAsync(UploadSongRequest request)
        {
            var uploadFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "uploads");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var fileName = $"{Guid.NewGuid()}.mp4";

            var filePath = Path.Combine(
                uploadFolder,
                fileName);

            await using var stream = new FileStream(
                filePath,
                FileMode.Create);

            await request.File.CopyToAsync(stream);

            var song = new Song
            {
                Title = request.Title,
                FileName = fileName,
                ContentType = "video/mp4",
                CreatedAt = DateTime.UtcNow
            };

            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            return song;
        }

        public async Task<FileStream?> GetSongStreamAsync(int id)
        {
            var song = await _context.Songs.FindAsync(id);

            if (song == null)
            {
                return null;
            }

            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "uploads",
                song.FileName);

            if (!File.Exists(filePath))
            {
                return null;
            }

            var stream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);

            return stream;
        }

        public async Task<bool> DeleteSongAsync(int id)
        {
            var song = await _context.Songs.FindAsync(id);

            if (song == null)
            {
                return false;
            }

            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "uploads",
                song.FileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}