using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CsScore.Models.Dto
{
    public class UserBulkCreateDto
    {
        [Required]
        public int TypeId { get; set; }

        [Required]
        public ICollection<UserInBulkCreateDto> Users { get; set; }
    }
}
