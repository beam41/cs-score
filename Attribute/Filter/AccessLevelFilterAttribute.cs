using System;
using System.Diagnostics;
using System.Linq;
using CsScore.Enums;
using CsScore.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace CsScore.Attribute.Filter
{
    public class AccessLevelFilterAttribute : ActionFilterAttribute
    {
        private readonly AccessLevel _level;

        public AccessLevelFilterAttribute(AccessLevel level = AccessLevel.None)
        {
            _level = level;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (_level == AccessLevel.None)
            {
                return;
            }

            var id = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            bool.TryParse(context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Access")?.Value, out var typeHasDashboardAccess);

            var user = new UserLoginTokenDto
            {
                Id = id,
                TypeHasDashboardAccess = typeHasDashboardAccess,
            };

            if (_level == AccessLevel.Admin && user.TypeHasDashboardAccess == false)
            {
                context.Result = new UnauthorizedObjectResult(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Status = StatusCodes.Status401Unauthorized,
                    Detail = "Invalid User Info",
                    Extensions =
                    {
                        {"traceId", Activity.Current?.Id ?? context.HttpContext.TraceIdentifier},
                    },
                });
                return;
            }

            context.HttpContext.Items.Add("User", user);
        }
    }
}
