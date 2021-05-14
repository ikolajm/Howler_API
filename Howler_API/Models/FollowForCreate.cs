using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Howler_API.Models
{
    public class FollowForCreate
    {
        public int followId { get; set; }
        public int followedById { get; set; }
    }
}
