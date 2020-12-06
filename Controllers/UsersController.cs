using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CsScore.Models;
using CsScore.Models.Context;
using CsScore.Models.Dto;
using CsScore.Services.Interfaces;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authorization;

namespace CsScore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _context;

        private readonly IRandomService _randomService;

        private readonly IAuthService _authService;

        public UsersController(DatabaseContext context, IRandomService randomService, IAuthService authService)
        {
            _context = context;
            _randomService = randomService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(UserCreateDto user)
        {
            var type = new Type { Id = user.TypeId };
            _context.Type.Attach(type);

            var newUser = new User()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password ?? _randomService.RandomPassword(6),
                Type = type,
            };

            _context.User.Add(newUser);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (await UserExists(user.Id))
                {
                    return Conflict();
                }
                
                throw;
            }

            return CreatedAtAction("GetUser", new { id = user.Id }, newUser);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> BulkCreateUser(UserBulkCreateDto bulkData)
        {
            var anyDuplicate = bulkData.Users.GroupBy(x => x.Id).Any(g => g.Count() > 1);

            if (anyDuplicate)
            {
                return BadRequest();
            }

            var type = new Type { Id = bulkData.TypeId };
            _context.Attach(type);

            var newUsers = bulkData.Users
                .Select(u => new User
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Password = u.Password ?? _randomService.RandomPassword(6),
                    Type = type,
                }).ToList();

            _context.AddRange(newUsers);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (await UserExists(newUsers))
                {
                    return Conflict();
                }

                throw;
            }

            return CreatedAtAction("GetUser", newUsers);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditType(string id, UserEditDto user)
        {
            var type = new Type { Id = user.TypeId };
            _context.Type.Attach(type);

            var editType = new User()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password ?? _randomService.RandomPassword(6),
                Type = type,
            };

            if (user.GroupId != null)
            {
                var group = new Group { Id = (int) user.GroupId };
                _context.Group.Attach(group);
                editType.Group = group;
            }
            

            

            _context.Entry(editType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(id))
                {
                    return NotFound();
                }

                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteType(string id)
        {
            var deleteType = new User { Id = id };

            _context.Entry(deleteType).State = EntityState.Deleted;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(id))
                {
                    return NotFound();
                }

                throw;
            }
            return NoContent();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserLoginInfoDto>> Login(UserLoginDto data)
        {
            var userResult = await _context.User
                .Where(u => u.Id == data.Id && u.Password == data.Password)
                .Select(u => new 
                {
                    u.Id,
                    u.Type.HasDashboardAccess
                }).FirstOrDefaultAsync();

            if (userResult == null)
            {
                throw new ProblemDetailsException(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Status = StatusCodes.Status401Unauthorized,
                    Detail = "Invalid User Info",
                });
            }

            var user = new UserLoginInfoDto
            {
                Id = userResult.Id,
                Token = _authService.GenToken(new UserLoginTokenDto
                {
                    Id = userResult.Id,
                    TypeHasDashboardAccess = userResult.HasDashboardAccess,
                }),
            };

            return user;
        }

        private Task<bool> UserExists(string id)
        {
            return _context.User.AnyAsync(e => e.Id == id);
        }

        private Task<bool> UserExists(IEnumerable<User> user)
        {
            return _context.User.AnyAsync(e => user.Any(u => u.Id == e.Id));
        }
    }
}
