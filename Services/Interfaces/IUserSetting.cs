using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsScore.Services.Interfaces
{
    public interface IUserSetting
    {
        public string Secret { get; set; }

        public string PasswordSalt { get; set; }
    }
}
