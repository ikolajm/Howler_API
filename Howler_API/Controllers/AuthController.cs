using Howler_API.Interfaces;
using Howler_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Howler_API.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        public AuthController(IAuthService authService, IConfiguration config)
        {
            _authService = authService;
            _config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserForRegister userForRegister)
        {
            if (!string.IsNullOrEmpty(userForRegister.username))
                userForRegister.username = userForRegister.username.ToLower();

            if (await _authService.UserExists(userForRegister.username))
                ModelState.AddModelError("Username", "Username already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            userForRegister.username = userForRegister.username.ToLower();

            if (await _authService.UserExists(userForRegister.username))
                return BadRequest("Username is already taken");

            var userToCreate = new User
            {
                email = userForRegister.email,
                name = userForRegister.name,
                username = userForRegister.username,
                createdAt = DateTime.Now,
                avatarBackground = userForRegister.avatarBackground
            };

            var createUser = await _authService.Register(userToCreate, userForRegister.password);

            //  GENERATE TOKEN
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor    //  Describes information we want to include in token
            {
                Subject = new ClaimsIdentity(new Claim[]        //  Payload
                {
            new Claim(ClaimTypes.NameIdentifier, createUser.Id.ToString()),
            new Claim(ClaimTypes.Name, createUser.username)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);  //  Create token
            var tokenString = tokenHandler.WriteToken(token);       //  to string (from byte[])

            return Ok(new { token = tokenString, user = createUser });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForLogin userForLogin)
        {
            var userFromDb = await _authService.Login(userForLogin.username.ToLower(), userForLogin.password);

            if (userFromDb == null)
                return Unauthorized();

            //  GENERATE TOKEN
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor    //  Describes information we want to include in token
            {
                Subject = new ClaimsIdentity(new Claim[]        //  Payload
                {
            new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
            new Claim(ClaimTypes.Name, userFromDb.username)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);  //  Create token
            var tokenString = tokenHandler.WriteToken(token);       //  to string (from byte[])

            return Ok(new { token = tokenString, user = userFromDb });                        //  Return 200, passing along token and user
        }

        // Find User by ID for persistant login
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> TokenGetUser(int id)
        {
            var user = await _authService.GetUserFromToken(id);
            return Ok(new { user = user });
        }
    }
}
