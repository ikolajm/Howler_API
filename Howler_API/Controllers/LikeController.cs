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
    [Route("api/like")]
    public class LikeController : Controller
    {
        private readonly ILikeService _likeService;
        private readonly IPostService _postService;
        private readonly IConfiguration _config;
        public LikeController(ILikeService likeService, IPostService postService, IConfiguration config)
        {
            _config = config;
            _likeService = likeService;
            _postService = postService;
        }

        // Create like
        [HttpPost]
        public async Task<object> CreateLike([FromBody] LikeForCreation like)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var likeToCreate = new Like
            {
                PostId = like.PostId,
                UserId = like.UserId
            };

            var post = await _postService.FindOneById(like.PostId);

            var createdLike = await _likeService.CreateLike(likeToCreate, post);

            var newPostData = await _postService.FindNewPostData(like.PostId, like.UserId);

            return newPostData;
        }

        // Delete like
        [HttpDelete("{id}")]
        public async Task<object> DeleteLike(int id)
        {
            var deleteLike = await _likeService.DeleteLike(id);

            return deleteLike;
        }
    }
}
