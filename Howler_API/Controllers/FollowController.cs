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
    [Route("api/follow")]
    public class FollowController : Controller
    {
        private readonly IFollowService _followService;
        private readonly IUserService _userService;
        private readonly IConfiguration _config;

        public FollowController(IFollowService followService, IUserService userService, IConfiguration config)
        {
            _config = config;
            _followService = followService;
            _userService = userService;
        }

        // Create Follow
        [HttpPost]
        public async Task<object> CreateFollow([FromBody] FollowForCreate followObj)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var followedUser = await _userService.GetUserById(followObj.followId);
            var followedByUser = await _userService.GetUserById(followObj.followedById);

            var postToCreate = new Post
            {
                content = postForUpload.content,
                imageURL = "",
                hidden = postForUpload.hidden,
                createdAt = DateTime.Now,
                edited = false,
                UserId = user.Id
            };

            var createPost = await _postService.CreatePost(postToCreate);
            return Ok(new
            {
                Id = createPost.Id,
                content = createPost.content,
                imageURL = createPost.imageURL,
                edited = createPost.edited,
                createdAt = createPost.createdAt,
                UserId = createPost.UserId,
                User = new
                {
                    Id = user.Id,
                    name = user.name,
                    email = user.email,
                    username = user.username,
                    avatarBackground = user.avatarBackground,
                    createdAt = user.createdAt
                }
            });
        }
    }
}
