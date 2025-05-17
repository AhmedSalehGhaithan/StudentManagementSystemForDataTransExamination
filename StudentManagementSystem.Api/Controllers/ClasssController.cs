using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Application.Features.Classes.Read;
using StudentManagementSystem.Application.Features.Classes.Write;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClassesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CreateClassCommandResponse>> CreateClass(CreateClassCommand command)
    {
        var classEntity = await mediator.Send(command);
        return Ok(classEntity);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Guid>> UpdateClass(UpdateClassCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteClass(Guid id)
    {
        await mediator.Send(new DeleteClassCommand(id));
        return NoContent();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ClassDto>> GetClassById(Guid id)
    {
        var classEntity = await mediator.Send(new GetClassByIdQuery(id));
        return Ok(classEntity);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ICollection<ClassDto>>> GetAllClasses()
    {
        var classes = await mediator.Send(new GetAllClassesQuery());
        return Ok(classes);
    }
}