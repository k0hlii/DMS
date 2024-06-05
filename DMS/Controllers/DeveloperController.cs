using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace DMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeveloperController : ControllerBase
    {
        private readonly DmsDbContext _context;

        public DeveloperController(DmsDbContext context)
        {
            _context = context;
        }

        // GET: api/Developer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Developer>>> GetDevelopers()
        {
            return await _context.Developers.ToListAsync();
        }

        // GET: api/Developer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Developer>> GetDeveloper(string id)
        {
            var developer = await _context.Developers.FindAsync(new ObjectId(id)); // Convert string to ObjectId

            if (developer == null)
            {
                return NotFound();
            }

            return developer;
        }

        // PUT: api/Developer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeveloper(string id, Developer developer)
        {
            developer._id = new ObjectId(id);
            
            //print the developer object
            Console.Error.WriteLine(developer);
            
            if (id != developer.IdAsString)
            {
                return BadRequest();
            }

            _context.Entry(developer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeveloperExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Developer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Developer>> PostDeveloper(Developer developer)
        {
            developer._id = new ObjectId(developer.IdAsString);

            Console.WriteLine(developer);
            _context.Developers.Add(developer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeveloper", new { id = developer._id }, developer);
        }

        // DELETE: api/Developer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeveloper(string id)
        {
            var developer = await _context.Developers.FindAsync(new ObjectId(id));
            if (developer == null)
            {
                return NotFound();
            }

            _context.Developers.Remove(developer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeveloperExists(string id)
        {
            return _context.Developers.Any(e => e.IdAsString== id);
        }
    }
}
