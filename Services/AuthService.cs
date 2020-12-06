using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CsScore.Models;
using CsScore.Models.Dto;
using CsScore.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace CsScore.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserSetting _userSetting;

        public AuthService(IUserSetting userSetting)
        {
            _userSetting = userSetting;
        }

        public string GenToken(UserLoginTokenDto tokenPayload)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_userSetting.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", tokenPayload.Id),
                    new Claim("Access", tokenPayload.TypeHasDashboardAccess.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
