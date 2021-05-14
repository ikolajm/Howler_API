using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Howler_API.Models
{
    public class CommentAuthor
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string avatarBackground { get; set; }
        [Required]
        public int postCount { get; set; }

    }
}
