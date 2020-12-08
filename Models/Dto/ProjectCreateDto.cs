using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CsScore.Models.Dto
{
    public class ProjectCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
