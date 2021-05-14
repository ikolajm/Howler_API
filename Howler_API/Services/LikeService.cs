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
    public class LikeService : ILikeService
    {
        private readonly DatabaseContext _context;
        public LikeService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<object> CreateLike(Like like, Post post)
        {
            await _context.Likes.AddAsync(like);
            post.Likes.Add(like);
            await _context.SaveChangesAsync();

            return like;
        }

        public async Task<Like> GetById(int id)
        {
            var like = await _context.Likes
                .Where(like => like.Id == id)
                .FirstOrDefaultAsync();
            return like;
        }

        public async Task<object> DeleteLike(int id)
        {
            var like = await _context.Likes
                .Where(like => like.Id == id)
                .FirstOrDefaultAsync();
            
            if (like == null)
            {
                return new { status = "Failure - returned null" };
            }
            
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return new { status = "Success" };
        }
    }
}
