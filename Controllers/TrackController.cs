using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebPlayer.DataBase.TrackDB;

namespace WebPlayer.Controllers
{
    [ApiController]
    public class TrackController : Controller
    {
        private MongoTrackRepository _trackRepository;
        public TrackController(MongoTrackRepository trackRepository)
        {
            _trackRepository = trackRepository;
        }
        [HttpGet("tracks/{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            //TODO: Проверить метод Get(Track)
            var track = _trackRepository.Find(id);
            if (track == null)
            {
                return NotFound();
            }
            return Json(track);
        }
        [HttpPost("tracks")]
        public IActionResult Post([FromBody] TrackEntity track)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            //TODO: Проверить метод Post(Track)
            if (_trackRepository.Find(track.Id) != null)
            {
                return BadRequest();
            }

            _trackRepository.Insert(track).GetAwaiter().GetResult();
            var newTrack = _trackRepository.Find(track.Id);
            return Created(newTrack.Id.ToString(), newTrack);
        }
        [HttpDelete("tracks/{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            //TODO: Проверить метод Delete(Track)
            var track = _trackRepository.Find(id);
            if (track == null)
            {
                return NotFound();
            }
            _trackRepository.Delete(id);
            return Ok(id);
        }
        [HttpPatch("tracks")]
        public IActionResult Update([FromBody] TrackEntity track)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            //TODO: Проверить метод Update(Track)
            if (_trackRepository.Find(track.Id) == null)
            {
                return NotFound();
            }
            _trackRepository.Update(track);
            return Ok(track);
        }

        [HttpGet("tracks/{id}/audio")]
        public IActionResult GetAudio([FromRoute] string id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            //TODO:Связать передаваемый файл с именем из БД
            return File(_trackRepository.DownloadAudio(id), "application/m4a", id + ".m4a");
        }
    }
}