using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsScore.Models
{
    public class Score
    {
        public int Id { get; set; }

        public User FromUser { get; set; }

        public Project ToProject { get; set; }

        public DateTimeOffset SubmitDate { get; set; }
    }
}
