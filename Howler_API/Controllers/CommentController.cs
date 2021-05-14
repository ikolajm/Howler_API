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
    [Route("api/comment")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        public CommentController(ICommentService commentService, IPostService postService, IUserService userService, IConfiguration config)
        {
            _config = config;
            _commentService = commentService;
            _postService = postService;
            _userService = userService;
        }

        // Create Comment
        [HttpPost]
        public async Task<object> CreateComment([FromBody] CommentForUpload commentForUpload)
        {
            var post = await _postService.FindOneById(commentForUpload.PostId);
            var user = await _userService.GetBaseUserById(commentForUpload.UserId);
            var author = await _userService.GetCommentAuthor(commentForUpload.UserId);

            var newComment = new Comment
            {
                content = commentForUpload.content,
                UserId = commentForUpload.UserId,
                PostId = commentForUpload.PostId,
                edited = false,
                createdAt = DateTime.Now,
            };

            var createdComment = await _commentService.CreateComment(newComment, post, user);

            var returnObj = new
            {
                Id = createdComment.Id,
                content = createdComment.content,
                createdAt = createdComment.createdAt,
                edited = createdComment.edited,
                PostId = createdComment.PostId,
                User = author
            };

            return returnObj;
        }

        // Update Comment
        [HttpPut("{id}")]
        public async Task<object> UpdateComment(int id, [FromBody] CommentForUpdate commentForUpdate)
        {
            var update = await _commentService.UpdateComment(id, commentForUpdate);

            return Ok(update);
        }

        // Delete Comment
        [HttpDelete("{id}")]
        public async Task<object> DeleteComment(int id)
        {
            var comment = await _commentService.DeleteComment(id);

            return comment;
        }
    }
}
