using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CsScore.Models
{
    public class Type
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int AvailableSubmit { get; set; }

        [Required]
        public int PointPerSubmit { get; set; }

        [Required]
        public bool HasDashboardAccess { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
