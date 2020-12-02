using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CsScore.Models;
using CsScore.Models.Context;
using CsScore.Models.Dto;
using CsScore.Services;

namespace CsScore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _context;

        private readonly RandomService _randomService;

        public UsersController(DatabaseContext context, RandomService randomService)
        {
            _context = context;
            _randomService = randomService;
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
