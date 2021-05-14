using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Howler_API.Models
{
    public class Follow
    {
        [Key]
        [Required]
        public long Id { get; set; }
        [Required]
        public int FollowingUserId { get; set; }

        [InverseProperty("Following")]
        public User Following { get; set; }
        [Required]
        public int FollowedByUserId { get; set; }

        [InverseProperty("FollowedBy")]
        public User FollowedBy { get; set; }
    }
}
