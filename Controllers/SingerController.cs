using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebPlayer.DataBase.SingerDB;

namespace WebPlayer.Controllers
{
    [ApiController]
    public class SingerController : Controller
    {
        private MongoSingerRepository _singerRepository;

        public SingerController(MongoSingerRepository singerRepository)
        {
            _singerRepository = singerRepository;
        }

        [HttpGet("singers/{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var singer = _singerRepository.Find(id);
            if (singer == null)
            {
                return NotFound();
            }

            return Json(singer);
        }

        [HttpPost("singers")]
        public IActionResult Post([FromBody] SingerEntity singer)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (_singerRepository.Find(singer.Id) != null)
            {
                return BadRequest();
            }

            _singerRepository.Insert(singer).GetAwaiter().GetResult();
            var newSinger = _singerRepository.Find(singer.Id);
            return Created(newSinger.Id.ToString(), newSinger);
        }

        [HttpDelete("singers/{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var playlist = _singerRepository.Find(id);
            if (playlist == null)
            {
                return NotFound();
            }

            _singerRepository.Delete(id);
            return Ok(id);
        }
        
        [HttpPatch("singers")]
        public IActionResult Update([FromBody] SingerEntity singer)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (_singerRepository.Find(singer.Id) == null)
            {
                return NotFound();
            }
            _singerRepository.Update(singer);
            return Ok(singer);
        }
    }
}