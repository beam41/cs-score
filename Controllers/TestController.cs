using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsScore.Attribute;
using CsScore.Attribute.Filter;
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

        public TestController()
        {
        }

        [HttpGet]
        [AccessLevelFilter(AccessLevel.Admin)]
        public ActionResult<UserLoginTokenDto> TestUserToken()
        {
            return HttpContext.Items["User"] as UserLoginTokenDto;
        }
    }
}
