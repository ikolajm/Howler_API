using Howler_API.Interfaces;
using Howler_API.Models;
using HowlerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Howler_API.Services
{
    public class CommentService : ICommentService
    {
        private readonly DatabaseContext _context;
        public CommentService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateComment(Comment comment, Post post, User user)
        {
            // Create Comment
            await _context.Comments.AddAsync(comment);
            user.Comments.Add(comment);
            post.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Return comment
            return comment;
        }

        public async Task<object> DeleteComment(int id)
        {
            var comment = await _context.Comments
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (comment == null)
            {
                return new { status = "Failure" };
            }
            else
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();

                return new { status = "Success" };
            }
        }

        public async Task<object> UpdateComment(int id, CommentForUpdate comment)
        {
            // Get the post content in DB
            var existingComment = await _context.Comments
                .Where(c => c.Id == id)
                .Include(c => c.User)
                    .ThenInclude(u => u.Posts)
                .FirstOrDefaultAsync();

            if (existingComment == null)
            {
                return new { status = "Failure" };
            }
            else
            {
                existingComment.content = comment.content;
                existingComment.edited = true;

                await _context.SaveChangesAsync();

                return new
                {
                    Id = existingComment.Id,
                    content = existingComment.content,
                    edited = existingComment.edited,
                    createdAt = existingComment.createdAt,
                    UserId = existingComment.UserId,
                    PostId = existingComment.PostId,
                    User = new
                    {
                        Id = existingComment.User.Id,
                        name = existingComment.User.name,
                        username = existingComment.User.username,
                        avatarBackground = existingComment.User.avatarBackground,
                        postCount = existingComment.User.Posts.Count()
                    }
                };
            }
        }
    }
}
