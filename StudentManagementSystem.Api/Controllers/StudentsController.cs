using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Application.Features.Students.Read;
using StudentManagementSystem.Application.Features.Students.Write;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.FeaturesServices.Students.Write;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CreateStudentCommandResponse>> CreateStudent(CreateStudentCommand command)
    {
        var student = await mediator.Send(command);
        return Ok(student);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Guid>> UpdateStudent(UpdateStudentCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteStudent(Guid id)
    {
        await mediator.Send(new DeleteStudentCommand(id));
        return NoContent();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<StudentDto>> GetStudentById(Guid id)
    {
        var student = await mediator.Send(new GetStudentByIdQuery(id));
        return Ok(student);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ICollection<StudentDto>>> GetAllStudents()
    {
        var students = await mediator.Send(new GetAllStudentsQuery());
        return Ok(students);
    }
}