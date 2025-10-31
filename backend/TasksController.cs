
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/projects/{projectId}/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _db;
    public TasksController(AppDbContext db) { _db = db; }

    private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [HttpGet]
    public async Task<IActionResult> GetTasks(int projectId)
    {
        var userId = GetUserId();
        var project = await _db.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);
        if (project == null) return NotFound();
        return Ok(project.Tasks);
    }

    [HttpPost]
    public async Task<IActionResult> AddTask(int projectId, TaskDto dto)
    {
        var userId = GetUserId();
        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);
        if (project == null) return NotFound();
        var t = new TaskItem { Title = dto.Title, DueDate = dto.DueDate, ProjectId = projectId };
        _db.Tasks.Add(t);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTasks), new { projectId = projectId }, t);
    }

    [HttpPut("{id}/toggle")]
    public async Task<IActionResult> Toggle(int projectId, int id)
    {
        var userId = GetUserId();
        var t = await _db.Tasks.Include(x => x.Project).FirstOrDefaultAsync(x => x.Id == id && x.ProjectId == projectId && x.Project!.UserId == userId);
        if (t == null) return NotFound();
        t.IsCompleted = !t.IsCompleted;
        await _db.SaveChangesAsync();
        return Ok(t);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int projectId, int id)
    {
        var userId = GetUserId();
        var t = await _db.Tasks.Include(x => x.Project).FirstOrDefaultAsync(x => x.Id == id && x.ProjectId == projectId && x.Project!.UserId == userId);
        if (t == null) return NotFound();
        _db.Tasks.Remove(t);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
