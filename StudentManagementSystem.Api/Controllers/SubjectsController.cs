using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Application.Features.Subjects.Read;
using StudentManagementSystem.Application.Features.Subjects.Write;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace StudentManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubjectController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateSubjectCommandResponse>> CreateSubject(CreateSubjectCommand command)
    {
        var subject = await mediator.Send(command);
        return Ok(subject);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> UpdateSubject(UpdateSubjectCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSubject(Guid id)
    {
        await mediator.Send(new DeleteSubjectCommand(id));
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubjectDto>> GetSubjectById(Guid id)
    {
        var subject = await mediator.Send(new GetSubjectByIdQuery(id));
        return Ok(subject);
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<SubjectDto>>> GetAllSubjects()
    {
        var subjects = await mediator.Send(new GetAllSubjectsQuery());
        return Ok(subjects);
    }
}