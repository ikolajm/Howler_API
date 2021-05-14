using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Howler_API.Models
{
    public class UserForRegister
    {
        [Required]
        [StringLength(26, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 26 characters.")]
        public string name { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 16 characters.")]
        public string username { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 16 characters.")]
        public string password { get; set; }
        [Required]
        public string avatarBackground { get; set; }
    }
}
