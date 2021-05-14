using HowlerAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Howler_API.Models
{
    public class Post
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(150, ErrorMessage = "Post content can only be up to 150 characters")]
        public string content { get; set; }
        public string imageURL { get; set; }
        [Required]
        public bool hidden { get; set; }
        [Required]
        public bool edited { get; set; }
        [Required]
        public DateTime createdAt { get; set; }

        // RELATIONSHIPS
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}
