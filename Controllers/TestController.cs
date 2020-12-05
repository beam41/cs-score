using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsScore.Attribute;
using CsScore.Enums;
using CsScore.Models.Dto;
using CsScore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CsScore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        private readonly IUserScopedService _userScopedService;

        public TestController(IUserScopedService userScopedService)
        {
            _userScopedService = userScopedService;
        }

        [HttpGet]
        [AccessLevel(AccessLevel.Admin)]
        public ActionResult<UserLoginTokenDto> TestUserToken()
        {
            return _userScopedService.User;
        }
    }
}
