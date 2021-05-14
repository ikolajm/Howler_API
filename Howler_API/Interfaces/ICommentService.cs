using Howler_API.Models;
using HowlerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Howler_API.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> CreateComment(Comment comment, Post post, User user);
        Task<object> UpdateComment(int id, CommentForUpdate comment);
        Task<object> DeleteComment(int id);
    }
}
