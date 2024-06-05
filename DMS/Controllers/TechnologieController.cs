using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace DMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TechnologieController : ControllerBase
{
    private readonly DmsDbContext _context;

    public TechnologieController(DmsDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Technologie>>> GetTechnologies()
    {
        return await _context.Technologies.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Technologie>> GetTechnologie(string id)
    {
        var technologie = await _context.Technologies.FindAsync(new ObjectId(id)); // Convert string to ObjectId

        if (technologie == null)
        {
            return NotFound();
        }

        return technologie; // Convert ObjectId to string
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTechnologie(string id, Technologie technologie)
    {
        technologie._id = new ObjectId(id);
        
        if (id != technologie.IdAsString)
        {
            return BadRequest();
        }

        _context.Entry(technologie).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TechnologieExists(id))
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

    [HttpPost]
    public async Task<ActionResult<Technologie>> PostTechnologie(Technologie technologie)
    {
        technologie._id = new ObjectId(technologie.IdAsString);

        Console.WriteLine(technologie);
        _context.Technologies.Add(technologie);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTechnologie", new { id = technologie._id }, technologie);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTechnologie(string id)
    {
        var technologie = await _context.Technologies.FindAsync(new ObjectId(id));
        if (technologie == null)
        {
            return NotFound();
        }

        _context.Technologies.Remove(technologie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TechnologieExists(string id)
    {
        return _context.Technologies.Any(e => e.IdAsString == id);
    }
}