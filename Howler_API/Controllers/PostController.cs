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
    [Route("api/post")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IConfiguration _config;
        public PostController(IPostService postService, IConfiguration config)
        {
            _config = config;
            _postService = postService;
        }

        // Create Post
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostForUpload postForUpload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _postService.GetPostAuthor(postForUpload.UserId);

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
            return Ok(new { 
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

        // Get all public posts with data
        [HttpPost]
        [Route("view/global")]
        public async Task<object> GetPosts([FromBody] UserIdToSearchWith userIdObj, UserIdToSearchWith authUserId)
        {
            var posts = await _postService.GetGlobals(userIdObj, authUserId);

            return Ok(posts);
        }

        // Edit Post
        [HttpPut("{id}")]
        public async Task<object> UpdatePost(int id, [FromBody]PostForUpdate postForEdit)
        {
            var update = await _postService.UpdatePost(id, postForEdit);

            return Ok(update);
        }

        // Delete Post
        [HttpDelete("{id}")]
        public async Task<object> DeletePost(int id)
        {
            var post = await _postService.DeletePost(id);

            return post;
        }

        // Get posts for a specified user page
        [HttpPost]
        [Route("user/{id}")]
        public async Task<object> GetPostsForUser([FromBody] UserIdToSearchWith userIdObj)
        {
            var posts = await _postService.GetPostsForUser(userIdObj);

            return Ok(posts);
        }

        [HttpPost]
        [Route("view/likes")]
        public async Task<object> GetUserLikes([FromBody] UserIdToSearchWith userIdObj)
        {
            var posts = await _postService.GetUserLikes(userIdObj);

            return Ok(posts);
        }
    }
}

