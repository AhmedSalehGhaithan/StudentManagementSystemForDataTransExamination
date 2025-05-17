using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Application.Features.Teachers.Read;
using StudentManagementSystem.Application.Features.Teachers.Write;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeacherController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateTeacherCommandResponse>> CreateTeacher(CreateTeacherCommand command)
    {
        var teacher = await mediator.Send(command);
        return Ok(teacher);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> UpdateTeacher(UpdateTeacherCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTeacher(Guid id)
    {
        await mediator.Send(new DeleteTeacherCommand(id));
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeacherDto>> GetTeacherById(Guid id)
    {
        var teacher = await mediator.Send(new GetTeacherByIdQuery(id));
        return Ok(teacher);
    }

    [HttpGet]
   
    public async Task<ActionResult<ICollection<TeacherDto>>> GetAllTeachers()
    {
        var teachers = await mediator.Send(new GetAllTeachersQuery());
        return Ok(teachers);
    }
}