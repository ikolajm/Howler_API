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
    public class PostService: IPostService
    {
        private readonly DatabaseContext _context;
        public PostService(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<Post> CreatePost(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return post;
        }

        public async Task<object> GetGlobals(UserIdToSearchWith userIdObj, UserIdToSearchWith authUserId)
        {
            var userLikes = await _context.Likes
                .Where(like => like.UserId == userIdObj.Id)
                .Select(like => like.PostId)
                .ToListAsync();
            
            var userComments = await _context.Comments
                .Where(comment => comment.UserId == userIdObj.Id)
                .Select(comment => comment.PostId)
                .ToListAsync();

            var posts = await _context.Posts
                .Where(post => !post.hidden)
                .Include(post => post.User)
                    .ThenInclude(user => user.Posts)
                .Include(post => post.Comments).ThenInclude(comment => comment.User).ThenInclude(user => user.Posts)
                // .Include(post => post.User)
                // .ThenInclude(user => user.FollowedBy).Select(follow => )
                .Include(post => post.Likes)
                .Select(post => new
                    {
                        Id = post.Id,
                        content = post.content,
                        imageURL = post.imageURL,
                        edited = post.edited,
                        hidden = post.hidden,
                        createdAt = post.createdAt,
                        UserId = post.UserId,
                        User = new
                        {
                            Id = post.User.Id,
                            name = post.User.name,
                            email = post.User.email,
                            username = post.User.username,
                            avatarBackground = post.User.avatarBackground,
                            createdAt = post.User.createdAt,
                            postCount = post.User.Posts.Count(),
                            // followData = FormatFollowData(post.User.FollowedBy, authUserId)
                        },
                        likeData = new {
                            quanity = post.Likes.Count(),
                            likedByUser = CheckUserPostLikeArray(userLikes, post.Id),
                            likeId = GetLikeId(post.Likes, post.Id)
                        },
                        commentData = new
                        {
                            quanity = post.Comments.Count(),
                            commentedByUser = CheckUserPostCommentArray(userComments, post.Id),
                            comments = FormatCommentCollection(post.Comments)
                        }
                    }
                )
                .OrderByDescending(p => p.Id)
                .ToListAsync();
            
            return posts;
        }

        // public static object FormatFollowData(followedByArray, authUserId)
        // {

        // }

        public static bool CheckUserPostLikeArray(List<int> array, long postId)
        {
            var index = array.FindIndex(id => id == postId);
            return index != -1;
        }

        public static bool CheckUserPostCommentArray(List<int> array, long postId)
        {

            var index = array.FindIndex(id => id == postId);
            return index != -1;
        }

        public static long GetLikeId(ICollection<Like>postLikes, long postId)
        {
            var like = postLikes
                .Where(like => like.PostId == postId)
                .Select(like => like.Id)
                .FirstOrDefault();
            return like;
        }

        public async Task<Post> FindOneById(int postId)
        {
            var post = await _context.Posts
                .Where(post => post.Id == postId)
                .FirstOrDefaultAsync();

            return post;
        }

        public static object FormatCommentCollection(ICollection<Comment> array)
        {
            if (array.Any())
            {
                var formatted = array.Select(comment => new
                    {
                        Id = comment.Id,
                        content = comment.content,
                        edited = comment.edited,
                        createdAt = comment.createdAt,
                        UserId = comment.UserId,
                        PostId = comment.PostId,
                        User = new
                        {
                            Id = comment.User.Id,
                            name = comment.User.name,
                            username = comment.User.username,
                            avatarBackground = comment.User.avatarBackground,
                            postCount = comment.User.Posts.Count()
                        }
                    }
                );

                return formatted;
            }
            else
            {
                return new { };
            }

        }

        public async Task<object> FindNewPostData(int postId, int userId)
        {
            var userLikes = await _context.Likes
                .Where(like => like.UserId == userId)
                .Select(like => like.PostId)
                .ToListAsync();
            
            var userComments = await _context.Comments
                .Where(comment => comment.UserId == userId)
                .Select(comment => comment.PostId)
                .ToListAsync();

            var post = await _context.Posts
                .Where(post => post.Id == postId)
                .Include(post => post.Likes)
                .Include(post => post.User)
                .Include(post => post.Comments).ThenInclude(comment => comment.User).ThenInclude(user => user.Posts)
                .Select(post => new
                    {
                        Id = post.Id,
                        content = post.content,
                        imageURL = post.imageURL,
                        edited = post.edited,
                        hidden = post.hidden,
                        createdAt = post.createdAt,
                        UserId = post.UserId,
                        User = new
                        {
                            Id = post.User.Id,
                            name = post.User.name,
                            email = post.User.email,
                            username = post.User.username,
                            avatarBackground = post.User.avatarBackground,
                            createdAt = post.User.createdAt
                        },
                        likeData = new
                        {
                            quanity = post.Likes.Count(),
                            likedByUser = CheckUserPostLikeArray(userLikes, post.Id),
                            likeId = GetLikeId(post.Likes, post.Id)
                        },
                        commentData = new
                        {
                            quanity = post.Comments.Count(),
                            commentedByUser = CheckUserPostCommentArray(userComments, post.Id),
                            comments = FormatCommentCollection(post.Comments)
                        }
                    }
                )
                .FirstOrDefaultAsync();

            return post;
        }

        public async Task<User> GetPostAuthor(long Id)
        {
            var user = await _context.Users
                .Where(u => u.Id == Id)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<object> UpdatePost(int id, PostForUpdate postForEdit)
        {
            var userLikes = await _context.Likes
                .Where(like => like.UserId == postForEdit.UserId)
                .Select(like => like.PostId)
                .ToListAsync();

            var userComments = await _context.Comments
                .Where(comment => comment.UserId == postForEdit.UserId)
                .Select(comment => comment.PostId)
                .ToListAsync();

            // Get the post content in DB
            var existingPost = await _context.Posts
                .Where(p => p.Id == id)
                .Include(p => p.Likes)
                .Include(p => p.User)
                .Include(p => p.Comments).ThenInclude(comment => comment.User).ThenInclude(user => user.Posts)
                .FirstOrDefaultAsync();

            if (existingPost == null)
            {
                return new { status = "Failure" };
            }
            else
            {
                existingPost.content = postForEdit.content;
                existingPost.hidden = postForEdit.hidden;
                existingPost.edited = postForEdit.edited;

                await _context.SaveChangesAsync();

                return new
                {
                    Id = existingPost.Id,
                    content = existingPost.content,
                    imageURL = existingPost.imageURL,
                    edited = existingPost.edited,
                    createdAt = existingPost.createdAt,
                    UserId = existingPost.UserId,
                    User = new
                    {
                        Id = existingPost.User.Id,
                        name = existingPost.User.name,
                        email = existingPost.User.email,
                        username = existingPost.User.username,
                        avatarBackground = existingPost.User.avatarBackground,
                        createdAt = existingPost.User.createdAt
                    },
                    likeData = new
                    {
                        quanity = existingPost.Likes.Count(),
                        likedByUser = CheckUserPostLikeArray(userLikes, existingPost.Id),
                        likeId = GetLikeId(existingPost.Likes, existingPost.Id)
                    },
                    commentData = new
                    {
                        quanity = existingPost.Comments.Count(),
                        commentedByUser = CheckUserPostCommentArray(userComments, existingPost.Id),
                        comments = FormatCommentCollection(existingPost.Comments)
                    }
                };
            }
        }
        public async Task<object> DeletePost(int id)
        {
            var post = await _context.Posts
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (post == null)
            {
                return new { status = "Failure" };
            }
            else
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                return new { status = "Success" };
            }
        }

        public async Task<object> GetPostsForUser(UserIdToSearchWith userIdObj)
        {
            var userLikes = await _context.Likes
                .Where(like => like.UserId == userIdObj.Id)
                .Select(like => like.PostId)
                .ToListAsync();

            var userComments = await _context.Comments
                    .Where(comment => comment.UserId == userIdObj.Id)
                    .Select(comment => comment.PostId)
                    .ToListAsync();

            var posts = await _context.Posts
                .Where(post => !post.hidden && post.UserId == userIdObj.Id)
                .Include(post => post.User)
                .Include(post => post.Likes)
                .Include(p => p.Comments).ThenInclude(comment => comment.User).ThenInclude(user => user.Posts)
                .Select(post => new
                {
                    Id = post.Id,
                    content = post.content,
                    imageURL = post.imageURL,
                    edited = post.edited,
                    hidden = post.hidden,
                    createdAt = post.createdAt,
                    UserId = post.UserId,
                    User = new
                    {
                        Id = post.User.Id,
                        name = post.User.name,
                        email = post.User.email,
                        username = post.User.username,
                        avatarBackground = post.User.avatarBackground,
                        createdAt = post.User.createdAt
                    },
                    likeData = new
                    {
                        quanity = post.Likes.Count(),
                        likedByUser = CheckUserPostLikeArray(userLikes, post.Id),
                        likeId = GetLikeId(post.Likes, post.Id)
                    },
                    commentData = new
                    {
                        quanity = post.Comments.Count(),
                        commentedByUser = CheckUserPostCommentArray(userComments, post.Id),
                        comments = FormatCommentCollection(post.Comments)
                    }
                }
                )
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return posts;
        }

        public async Task<object> GetUserLikes(UserIdToSearchWith userIdObj)
        {
            var userLikes = await _context.Likes
                .Where(like => like.UserId == userIdObj.Id)
                .Select(like => like.PostId)
                .ToListAsync();

            var userComments = await _context.Comments
                .Where(comment => comment.UserId == userIdObj.Id)
                .Select(comment => comment.PostId)
                .ToListAsync();

            var posts = await _context.Posts
                .Where(post => userLikes.Contains(post.Id))
                .Include(post => post.User)
                .Include(post => post.Likes)
                .Include(p => p.Comments).ThenInclude(comment => comment.User).ThenInclude(user => user.Posts)
                .Select(post => new
                {
                    Id = post.Id,
                    content = post.content,
                    imageURL = post.imageURL,
                    edited = post.edited,
                    hidden = post.hidden,
                    createdAt = post.createdAt,
                    UserId = post.UserId,
                    User = new
                    {
                        Id = post.User.Id,
                        name = post.User.name,
                        email = post.User.email,
                        username = post.User.username,
                        avatarBackground = post.User.avatarBackground,
                        createdAt = post.User.createdAt
                    },
                    likeData = new
                    {
                        quanity = post.Likes.Count(),
                        likedByUser = CheckUserPostLikeArray(userLikes, post.Id),
                        likeId = GetLikeId(post.Likes, post.Id)
                    },
                    commentData = new
                    {
                        quanity = post.Comments.Count(),
                        commentedByUser = CheckUserPostCommentArray(userComments, post.Id),
                        comments = FormatCommentCollection(post.Comments)
                    }
                }
                )
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return posts;
        }
    }
}
