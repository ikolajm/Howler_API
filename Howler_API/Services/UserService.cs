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
    public class UserService: IUserService
    {
        private readonly DatabaseContext _context;
        public UserService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<object> GetUserById(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Include(u => u.Posts)
                // .Include(u => u.FollowedBy)
                .Select(u => new
                {
                    Id = u.Id,
                    name = u.name,
                    username = u.username,
                    userBackground = u.avatarBackground,
                    // followCount = u.FollowedBy.Count(),
                    postCount = u.Posts.Count()
                })
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<User> GetBaseUserById(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<object> GetCommentAuthor(int id)
        {
            var user = await _context.Users
                .Include(user => user.Posts)
                .Select(user => new CommentAuthor
                {
                    Id= user.Id,
                    username = user.username,
                    name = user.name,
                    avatarBackground = user.avatarBackground,
                    postCount = user.Posts.Count()
                })
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<object> UpdateUser(int id, UserForEdit userForEdit)
        {
            // Get the post content in DB
            var existingUser = await _context.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (existingUser == null)
            {
                return new { status = "Failure" };
            }
            else
            {
                existingUser.name = userForEdit.name;
                existingUser.username = userForEdit.username;
                existingUser.avatarBackground = userForEdit.avatarBackground;

                await _context.SaveChangesAsync();

                var updatedUser = await _context.Users
                .Where(u => u.Id == id)
                .Include(u => u.Posts)
                // .Include(u => u.FollowedBy)
                .Select(u => new
                {
                    Id = u.Id,
                    name = u.name,
                    username = u.username,
                    userBackground = u.avatarBackground,
                    // followCount = u.FollowedBy.Count(),
                    postCount = u.Posts.Count()
                })
                .FirstOrDefaultAsync();

                return updatedUser;
            }
        }
    }
}
