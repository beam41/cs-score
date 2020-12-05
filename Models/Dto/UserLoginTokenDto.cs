using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsScore.Models.Dto
{
    public class UserLoginTokenDto
    {
        public string Id { get; set; }

        public bool TypeHasDashboardAccess { get; set; }
    }
}
