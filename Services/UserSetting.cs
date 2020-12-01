using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsScore.Services.Interfaces;

namespace CsScore.Services
{
    public class UserSetting : IUserSetting
    {
        public string Secret { get; set; }

        public string SuperUserPassword { get; set; }
    }
}
