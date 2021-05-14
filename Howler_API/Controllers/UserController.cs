using Howler_API.Interfaces;
using Howler_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Howler_API.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        public UserController(IUserService userService, IConfiguration config)
        {
            _config = config;
            _userService = userService;
        }

        [HttpPost]
        [Route("view/{id}")]
        public async Task<object> GetUserById([FromBody] UserIdToSearchWith userIdObj)
        {
            var id = userIdObj.Id;
            var user = await _userService.GetUserById(id);

            return user;
        }

        [HttpPut]
        [Route("edit/{id}")]
        public async Task<object> UpdateUser(int id, [FromBody] UserForEdit userEditObj)
        {
            var user = await _userService.UpdateUser(id, userEditObj);

            return user;
        }
    }
}
