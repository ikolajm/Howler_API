using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Howler_API.Models;

namespace Howler_API.Interfaces
{
    public interface ILikeService
    {
        Task<object> CreateLike(Like like, Post post);
        Task<object> DeleteLike(int id);
        Task<Like> GetById(int id);
    }
}
