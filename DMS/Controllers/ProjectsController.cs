using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace DMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly DmsDbContext _context;

    public ProjectController(DmsDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
    {
        return await _context.Projects.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetProject(string id)
    {
        var project = await _context.Projects.FindAsync(new ObjectId(id)); // Convert string to ObjectId

        if (project == null)
        {
            return NotFound();
        }

        return project; // Convert ObjectId to string
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProject(string id, Project project)
    {
        project._id = new ObjectId(id);
        
        if (id != project.IdAsString)
        {
            return BadRequest();
        }

        _context.Entry(project).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProjectExists(id))
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
    public async Task<ActionResult<Project>> PostProject(Project project)
    {
        project._id = new ObjectId(project.IdAsString);

        Console.WriteLine(project);
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProject", new { id = project._id }, project);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(string id)
    {
        var project = await _context.Projects.FindAsync(new ObjectId(id));
        if (project == null)
        {
            return NotFound();
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProjectExists(string id)
    {
        return _context.Projects.Any(e => e.IdAsString == id);
    }
}