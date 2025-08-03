using Application.Commands;
using Application.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetAll()
    {
        var projects = await mediator.Send(new GetProjectsQuery());
        return Ok(projects);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetById(string id)
    {
        var project = await mediator.Send(new GetProjectByIdQuery(id));
        if (project == null) return NotFound();
        return Ok(project);
    }
    
    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject([FromBody] Project project)
    {
        var created = await mediator.Send(new CreateProjectCommand(project));
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(string id, [FromBody] Project project)
    {
        var updated = await mediator.Send(new UpdateProjectCommand(id, project));
        if (!updated) return NotFound();
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(string id)
    {
        var deleted = await mediator.Send(new DeleteProjectCommand(id));
        if (!deleted) return NotFound();
        return NoContent();
    }
}