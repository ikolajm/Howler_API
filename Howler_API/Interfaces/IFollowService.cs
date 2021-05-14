using Howler_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Howler_API.Interfaces
{
    public interface IFollowService
    {
        Task<object> FollowUser(Follow followObj);
        Task<object> UnfollowUser(int id);
    }
}
