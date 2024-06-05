using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace DMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectMemberController : ControllerBase
{
    private readonly DmsDbContext _context;

    public ProjectMemberController(DmsDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectMember>>> GetProjectMembers()
    {
        return await _context.ProjectMembers.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectMember>> GetProjectMember(string id)
    {
        var projectMember = await _context.ProjectMembers.FindAsync(new ObjectId(id)); // Convert string to ObjectId

        if (projectMember == null)
        {
            return NotFound();
        }

        return projectMember; // Convert ObjectId to string
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProjectMember(string id, ProjectMember projectMember)
    {
        projectMember._id = new ObjectId(id);

        if (id != projectMember.IdAsString)
        {
            return BadRequest();
        }

        _context.Entry(projectMember).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProjectMemberExists(id))
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
    public async Task<ActionResult<ProjectMember>> PostProjectMember(ProjectMember projectMember)
    {
        projectMember._id = new ObjectId(projectMember.IdAsString);

        Console.WriteLine(projectMember);
        _context.ProjectMembers.Add(projectMember);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProjectMember", new { id = projectMember._id }, projectMember);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProjectMember(string id)
    {
        var projectMember = await _context.ProjectMembers.FindAsync(new ObjectId(id));
        if (projectMember == null)
        {
            return NotFound();
        }

        _context.ProjectMembers.Remove(projectMember);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProjectMemberExists(string id)
    {
        return _context.ProjectMembers.Any(e => e.IdAsString == id);
    }
}