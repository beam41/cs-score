using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsScore.Models;
using CsScore.Models.Dto;
using CsScore.Services.Interfaces;

namespace CsScore.Services
{
    public class UserScopedService : IUserScopedService
    {
        public UserLoginTokenDto User { get; set; }
    }
}
