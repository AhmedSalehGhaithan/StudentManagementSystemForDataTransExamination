using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Application.Features.Schedules.Read;
using StudentManagementSystem.Application.Features.Schedules.Write;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace StudentManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScheduleController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateScheduleCommandResponse>> CreateSchedule(CreateScheduleCommand command)
    {
        var schedule = await mediator.Send(command);
        return Ok(schedule);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> UpdateSchedule(UpdateScheduleCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSchedule(Guid id)
    {
        await mediator.Send(new DeleteScheduleCommand(id));
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ScheduleDto>> GetScheduleById(Guid id)
    {
        var schedule = await mediator.Send(new GetScheduleByIdQuery(id));
        return Ok(schedule);
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<ScheduleDto>>> GetAllSchedules()
    {
        var schedules = await mediator.Send(new GetAllSchedulesQuery());
        return Ok(schedules);
    }
}