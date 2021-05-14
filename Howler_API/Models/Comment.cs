using Howler_API.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Howler_API.Models
{
    public class Comment
    {
        [Key]
        [Required]
        public long Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Comment can only be a max of 100 characters")]
        public string content { get; set; }
        //CreatedAt
        [Required]
        public DateTime createdAt { get; set; }
        [Required]
        public bool edited { get; set; }

        // RELATIONSHIPS
        public int UserId { get; set; }
        public User User { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}