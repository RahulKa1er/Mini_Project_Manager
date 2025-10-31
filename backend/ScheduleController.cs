
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/v1/projects/{projectId}/schedule")]
public class ScheduleController : ControllerBase
{
    private readonly AppDbContext _db;
    public ScheduleController(AppDbContext db) { _db = db; }

    private int GetUserId() =>
        int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [HttpPost]
    public async Task<IActionResult> GenerateSchedule(int projectId)
    {
        var userId = GetUserId();

        var project = await _db.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

        if (project == null)
            return NotFound("Project not found");

        // Sort by completion and due date
        var ordered = project.Tasks
            .OrderBy(t => t.IsCompleted)
            .ThenBy(t => t.DueDate ?? DateTime.MaxValue)
            .ToList();

        for (int i = 0; i < ordered.Count; i++)
            ordered[i].Title = $"{i + 1}. {ordered[i].Title}";

        return Ok(new
        {
            project = project.Title,
            generatedAt = DateTime.UtcNow,
            orderedTasks = ordered.Select(t => new
            {
                t.Id,
                t.Title,
                t.DueDate,
                t.IsCompleted
            })
        });
    }
}
