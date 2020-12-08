using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsScore.Attribute.Filter;
using CsScore.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CsScore.Models;
using CsScore.Models.Context;
using CsScore.Models.Dto;
using Hellang.Middleware.ProblemDetails;

namespace CsScore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ProjectsController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Project.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        [HttpPost]
        [AccessLevelFilter(AccessLevel.User)]
        public async Task<IActionResult> CreateProject(ProjectCreateDto project)
        {
            var user = HttpContext.Items["User"] as UserLoginTokenDto;

            var userInfo = await  _context.User
                .Where(u => u.Id == user.Id)
                .Select(u => new
                {
                    u.Id,
                    GroupId = u.Group.Id,
                }).FirstOrDefaultAsync();

            if (userInfo == null)
            {
                throw new ProblemDetailsException(new ProblemDetails
                {
                    Title = "Forbidden",
                    Status = StatusCodes.Status403Forbidden,
                    Detail = "Invalid Group",
                });
            }

            var newProject = new Project
            {
                Name = project.Name,
                GroupId = userInfo.GroupId,
            };

            _context.Add(newProject);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { newProject.Id },newProject);
        }

        [HttpPut("{id}")]
        [AccessLevelFilter(AccessLevel.User)]
        public async Task<IActionResult> RenameProject(int id, ProjectCreateDto project)
        {
            var user = HttpContext.Items["User"] as UserLoginTokenDto;

            var userInfo = await _context.User
                .Where(u => u.Id == user.Id)
                .Select(u => new
                {
                    GroupId = u.Group.Id,
                    ProjectId = u.Group.GroupProject.Id,
                }).FirstOrDefaultAsync();

            if (userInfo == null || id != userInfo.ProjectId)
            {
                throw new ProblemDetailsException(new ProblemDetails
                {
                    Title = "Forbidden",
                    Status = StatusCodes.Status403Forbidden,
                    Detail = "Invalid Project",
                });
            }

            var newProject = new Project
            {
                Id = userInfo.ProjectId,
                GroupId = userInfo.GroupId,
                Name = project.Name,
                UpdatedDate = new DateTimeOffset(),
            };

            _context.Entry(newProject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProjectExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        private Task<bool> ProjectExists(int id)
        {
            return _context.Project.AnyAsync(e => e.Id == id);
        }
    }
}
