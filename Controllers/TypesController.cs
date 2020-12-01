using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CsScore.Models;
using CsScore.Models.Context;
using CsScore.Models.Dto;

namespace CsScore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public TypesController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Type>> GetTypeById(int id)
        {
            var type = await _context.Type.FindAsync(id);

            if (type == null) return NotFound();

            return type;
        }

        [HttpPost]
        public async Task<ActionResult<Type>> CreateType(TypeCreateDto type)
        {
            var newType = new Type
            {
                Name = type.Name,
                AvailableSubmit = type.AvailableSubmit,
                PointPerSubmit = type.PointPerSubmit,
                HasDashboardAccess = type.HasDashboardAccess,
            };

            _context.Type.Add(newType);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeById", new { newType.Id }, newType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditType(int id, TypeCreateDto type)
        {
            var editType = new Type
            {
                Id = id,
                Name = type.Name,
                AvailableSubmit = type.AvailableSubmit,
                PointPerSubmit = type.PointPerSubmit,
                HasDashboardAccess = type.HasDashboardAccess,
            };

            _context.Entry(editType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TypeExists(id))
                {
                    return NotFound();
                }

                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteType(int id)
        {
            var deleteType = new Type { Id = id };

            _context.Entry(deleteType).State = EntityState.Deleted;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TypeExists(id))
                {
                    return NotFound();
                }

                throw;
            }
            return NoContent();
        }

        private Task<bool> TypeExists(int id)
        {
            return _context.Type.AnyAsync(t => t.Id == id);
        }
    }
}
