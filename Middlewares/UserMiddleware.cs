using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CsScore.Attribute;
using CsScore.Enums;
using CsScore.Models;
using CsScore.Models.Dto;
using CsScore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;

namespace CsScore.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class UserMiddleware
    {
        private readonly RequestDelegate _next;

        public UserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IUserScopedService userScopedService)
        {
            var endpoint = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            var allowAnonymous = endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null;
            var access = endpoint?.Metadata.GetMetadata<AccessLevelAttribute>();

            if (allowAnonymous || access == null || access.Level == AccessLevel.None)
            {
                await _next.Invoke(httpContext);
                return;
            }

            var userStr = httpContext.User.Claims.FirstOrDefault(c => c.Type == "User")?.Value;

            var user = JsonConvert.DeserializeObject<UserLoginTokenDto>(userStr);

            if (access.Level == AccessLevel.Admin && user.TypeHasDashboardAccess == false)
            {
                httpContext.Response.Clear();
                httpContext.Response.ContentType = "text/plain";
                httpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsync("Invalid Access");
                return;
            }

            userScopedService.User = user;

            await _next.Invoke(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class UserMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserMiddleware>();
        }
    }
}
