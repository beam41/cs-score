using CsScore.Models.Dto;

namespace CsScore.Services.Interfaces
{
    public interface IAuthService
    {
        public string GenToken(UserLoginTokenDto tokenPayload);
    }
}
