using Howler_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Howler_API.Interfaces
{
    public interface IPostService
    {
        Task<Post> CreatePost(Post post);
        Task<object> GetGlobals(UserIdToSearchWith userIdObj, UserIdToSearchWith authUserId);
        Task<object> GetUserLikes(UserIdToSearchWith userIdObj);
        Task<Post> FindOneById(int postId);
        Task<object> FindNewPostData(int postId, int userId);
        Task<User> GetPostAuthor(long Id);
        Task<object> UpdatePost(int id, PostForUpdate postForEdit);
        Task<object> DeletePost(int id);
        Task<object> GetPostsForUser(UserIdToSearchWith userIdObj);
    }
}
