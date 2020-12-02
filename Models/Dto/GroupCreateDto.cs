using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CsScore.Models.Dto
{
    public class GroupCreateDto
    {
        public int Id { get; set; }

        public ICollection<string> UsersIdInGroup { get; set; }
    }
}
