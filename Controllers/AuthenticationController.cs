using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebPlayer.DataBase.UserDB;

namespace WebPlayer.Controllers
{
    public class LoginModel
    {
        // [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }

        // [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        // [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }

        // [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }

        // [Required(ErrorMessage = "Не указан email")]
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class AuthenticationController : Controller
    {
        private MongoUserRepository _userRepository;

        public AuthenticationController(MongoUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("authentication/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel.Login != String.Empty && loginModel.Password != String.Empty)
            {
                var user = _userRepository.Find(loginModel.Login);
                if (user == null)
                {
                    return NotFound(loginModel);
                }

                if (user.Password.CompareTo(loginModel.Password) != 0)
                {
                    return NotFound(loginModel);
                }

                await Authenticate(loginModel.Login);
                return Ok(loginModel);
            }

            return BadRequest("Отсутствует логин и/или пароль.");
        }

        [HttpPost("authentication/register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            var user = _userRepository.Find(registerModel.Login);
            if (user != null)
            {
                return BadRequest();
            }

            user = new UserEntity(registerModel.Login, registerModel.Password, registerModel.Email, registerModel.Name);
            _userRepository.Insert(user);
            await Authenticate(user.Login);
            return Created(user.Login, user);
        }

        public async Task Authenticate(string login)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login)
            };
            var identity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }
    }
}