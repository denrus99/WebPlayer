using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WebPlayer.DataBase.AlbumDB;

namespace WebPlayer.Controllers
{
    [ApiController]
    public class AlbumController : Controller
    {
        private MongoAlbumRepository _albumRepository;
        public AlbumController(MongoAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }
        [HttpGet("albums/{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var album = _albumRepository.Find(id);
            if (album == null)
            {
                return NotFound();
            }
            return Json(album);
        }
        [HttpGet("albums")]
        public async Task<IActionResult> GetByTrack([FromQuery] string trackId)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var album = await _albumRepository.FindByTrack(new ObjectId(trackId));
            if (album == null)
            {
                return NotFound();
            }
            return Json(album);
        }
        [HttpPost("albums")]
        public IActionResult Post([FromBody] AlbumEntity album)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (_albumRepository.Find(album.Id) != null)
            {
                return BadRequest();
            }

            _albumRepository.Insert(album).GetAwaiter().GetResult();
            var newAlbum = _albumRepository.Find(album.Id);
            return Created(newAlbum.Id.ToString(), newAlbum);
        }
        [HttpDelete("albums/{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var album = _albumRepository.Find(id);
            if (album == null)
            {
                return NotFound();
            }
            _albumRepository.Delete(id);
            return Ok(id);
        }
        [HttpPatch("albums")]
        public IActionResult Update([FromBody] AlbumEntity album)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (_albumRepository.Find(album.Id) == null)
            {
                return NotFound();
            }
            _albumRepository.Update(album);
            return Ok(album);
        }
    }
}