using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Howler_API.Models
{
    public class PostForUpload
    {
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Post content can be 1-150 characters max.")]
        public string content { get; set; }
        public bool hidden { get; set; }
        public int UserId { get; set; }
    }
}
