using Microsoft.AspNetCore.Mvc;
using WebPlayer.DataBase.UserDB;

namespace WebPlayer.Controllers
{
    [ApiController]
    public class UserController : Controller
    {
        private MongoUserRepository _userRepository;

        public UserController(MongoUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet("users/{login}")]
        public IActionResult Get([FromRoute] string login)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var user = _userRepository.Find(login);
            if (user == null)
            {
                return NotFound();
            }
            return Json(user);
        }
        [HttpPost("users")]
        public IActionResult Post([FromBody] UserEntity user)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (_userRepository.Find(user.Login) != null)
            {
                return BadRequest();
            }

            _userRepository.Insert(user).GetAwaiter().GetResult();
            var newUser = _userRepository.Find(user.Login);
            return Created(newUser.Login, newUser);
        }
        [HttpDelete("users/{login}")]
        public IActionResult Delete([FromRoute] string login)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var user = _userRepository.Find(login);
            if (user == null)
            {
                return NotFound();
            }
            _userRepository.Delete(login);
            return Ok(login);
        }
        [HttpPatch("users")]
        public IActionResult Update([FromBody] UserEntity user)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (_userRepository.Find(user.Login) == null)
            {
                return NotFound();
            }
            _userRepository.Update(user);
            return Ok(user);
        }
    }
}