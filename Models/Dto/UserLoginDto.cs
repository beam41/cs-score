using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CsScore.Models.Dto
{
    public class UserLoginDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
