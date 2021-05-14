using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Howler_API.Models
{
    public class Like
    {
        [Key]
        [Required]
        public long Id { get; set; }

        // RELATIONSHIPS
        public int PostId { get; set; }
        public int UserId { get; set; }
        
    }
}
