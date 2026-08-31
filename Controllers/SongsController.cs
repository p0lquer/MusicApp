using Microsoft.AspNetCore.Mvc;
using MusicApp.Models;
using MusicApp.Services;

namespace MusicApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongsController : Controller
    {
        private readonly SongsService _songsService;

        public SongsController(SongsService songsService)
        {
            _songsService = songsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Song>>> GetSongs()
        {
            var songs = await _songsService.GetSongAsync();

            return Ok(songs);
        }

        [HttpPost]
        public async Task<ActionResult<Song>> UploadSong(
            [FromForm] UploadSongRequest request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest("Esta Vacio");
            }
            if (!Path.GetExtension(request.File.FileName)
                .Equals(".mp4", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Debe ser mp4");
            }

            var song = await _songsService.UploadSongAsync(request);

            return CreatedAtAction(
                nameof(GetSongs),
                new
                {
                    id = song.Id
                },
                song);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Song>> GetSong(int id)
        {
            var song = await _songsService.GetSongByIdAsync(id);

            if (song == null)
            {
                return NotFound();
            }

            return Ok(song);
        }

        [HttpGet("{id}/play")]
        public async Task<IActionResult> PlaySong(int id)
        {
            var song = await _songsService.GetSongByIdAsync(id);

            if (song == null)
            {
                return NotFound();
            }

            var stream = await _songsService.GetSongStreamAsync(id);

            if (stream == null)
            {
                return NotFound();
            }

            return File(
                stream,
                song.ContentType,
                enableRangeProcessing: true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            var deleted = await _songsService.DeleteSongAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}