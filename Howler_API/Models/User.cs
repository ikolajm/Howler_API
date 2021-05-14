using HowlerAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Howler_API.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public byte[] passwordHash { get; set; }
        [Required]
        public byte[] passwordSalt { get; set; }
        [Required]
        public string avatarBackground { get; set; }
        //CreatedAt
        [Required]
        public DateTime createdAt { get; set; }

        // RELATIONSHIPS
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Follow> Following { get; set; }
        public ICollection<Follow> FollowedBy { get; set; }
    }
}
