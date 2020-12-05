using CsScore.Models;
using CsScore.Models.Dto;

namespace CsScore.Services.Interfaces
{
    public interface IUserScopedService
    {
        public UserLoginTokenDto User { get; set; }
    }
}
