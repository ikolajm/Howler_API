using Howler_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Howler_API.Interfaces
{
    public interface IUserService
    {
        Task<object> GetUserById(int id);
        Task<User> GetBaseUserById(int id);
        Task<object> GetCommentAuthor(int id);
        Task<object> UpdateUser(int id, UserForEdit userForEdit);
    }
}
