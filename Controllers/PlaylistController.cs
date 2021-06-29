using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WebPlayer.DataBase.PlaylistDB;

namespace WebPlayer.Controllers
{
    [ApiController]
    public class PlaylistController : Controller
    {
        private MongoPlaylistRepository _playlistRepository;
        public PlaylistController(MongoPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }
        [HttpGet("playlists/{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var playlist = _playlistRepository.Find(id);
            if (playlist == null)
            {
                return NotFound();
            }
            return Json(playlist);
        }
        [HttpGet("playlists/popular")]
        public async Task<IActionResult> GetByTrack([FromQuery] int count)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var playlists = _playlistRepository.FindPopular(count);
            if (playlists == null)
            {
                return NotFound();
            }
            return Json(playlists);
        }
        [HttpPost("playlists")]
        public IActionResult Post([FromBody] PlaylistEntity playlist)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (_playlistRepository.Find(playlist.Id) != null)
            {
                return BadRequest();
            }

            _playlistRepository.Insert(playlist).GetAwaiter().GetResult();
            var newPlaylist = _playlistRepository.Find(playlist.Id);
            return Created(newPlaylist.Id.ToString(), newPlaylist);
        }
        [HttpDelete("playlists/{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var playlist = _playlistRepository.Find(id);
            if (playlist == null)
            {
                return NotFound();
            }
            _playlistRepository.Delete(id);
            return Ok(id);
        }
        [HttpPatch("playlists")]
        public IActionResult Update([FromBody] PlaylistEntity playlist)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (_playlistRepository.Find(playlist.Id) == null)
            {
                return NotFound();
            }
            _playlistRepository.Update(playlist);
            return Ok(playlist);
        }
    }
}