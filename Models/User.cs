using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CsScore.Models
{
    public class User
    {
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        public Group Group { get; set; }

        [Required]
        public Type Type { get; set; }

        public ICollection<Score> ScoreRecords { get; set; }
    }
}
