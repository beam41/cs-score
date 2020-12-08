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
    public class GroupsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public GroupsController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Group>>> GetGroup()
        {
            return new List<Group>();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroup(int id)
        {
            var group = await _context.Group.FindAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            return group;
        }

        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup(IEnumerable<GroupCreateDto> groups)
        {
            var newGroups = groups.Select(g =>
            {
                var user = g.UsersIdInGroup.Select(id => new User { Id = id }).ToList();
                _context.AttachRange(user);
                return new Group
                {
                    Id = g.Id,
                    UsersInGroup = user,
                };
            });

            _context.AddRange(newGroups);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroup", newGroups);
        }

        [HttpPut("{id}")]
        [AccessLevelFilter(AccessLevel.User)]
        public async Task<ActionResult<Group>> EditName(int id, GroupEditName group)
        {
            var user = HttpContext.Items["User"] as UserLoginTokenDto;

            if (!await GroupExists(id, user?.Id))
            {
                throw new ProblemDetailsException(new ProblemDetails
                {
                    Title = "Forbidden",
                    Status = StatusCodes.Status403Forbidden,
                    Detail = "Invalid Group",
                });
            }

            var newGroups = new Group
            {
                Id = id,
                Name = group.Name,
            };

            _context.Entry(newGroups).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await GroupExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        private Task<bool> GroupExists(int id)
        {
            return _context.Group.AnyAsync(e => e.Id == id);
        }

        private Task<bool> GroupExists(int id, string userId)
        {
            return _context.Group.AnyAsync(e => e.Id == id && e.UsersInGroup.Any(user => user.Id == userId));
        }
    }
}
