using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Application.Features.Grades.Read;
using StudentManagementSystem.Application.Features.Grades.Write;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace StudentManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GradeController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateGradeCommandResponse>> CreateGrade(CreateGradeCommand command)
    {
        var grade = await mediator.Send(command);
        return Ok(grade);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> UpdateGrade(UpdateGradeCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGrade(Guid id)
    {
        await mediator.Send(new DeleteGradeCommand(id));
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GradeDto>> GetGradeById(Guid id)
    {
        var grade = await mediator.Send(new GetGradeByIdQuery(id));
        return Ok(grade);
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<GradeDto>>> GetAllGrades()
    {
        var grades = await mediator.Send(new GetAllGradesQuery());
        return Ok(grades);
    }
}