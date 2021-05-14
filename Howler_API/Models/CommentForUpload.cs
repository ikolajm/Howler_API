using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Howler_API.Models
{
    public class CommentForUpload
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Comment can only be a max of 100 characters")]
        public string content { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}
