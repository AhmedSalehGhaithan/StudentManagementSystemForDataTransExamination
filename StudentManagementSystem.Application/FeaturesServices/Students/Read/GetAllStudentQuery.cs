using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Domain.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace StudentManagementSystem.Application.Features.Students.Read;

public record GetAllStudentsQuery : IQuery<ICollection<StudentDto>>;

public class GetAllStudentsQueryHandler(IStudentRepository _repo)
    : IQueryHandler<GetAllStudentsQuery, ICollection<StudentDto>>
{
    public async Task<ICollection<StudentDto>> Handle(
        GetAllStudentsQuery request,
        CancellationToken cancellationToken)
    {
        var students = await _repo.GetAllAsync(q =>
            q.Include(s => s.Class)
             .Include(s => s.Grades),
            cancellationToken);

        return students.Select(s => new StudentDto(
            s.StudentId,
            s.FirstName,
            s.LastName,
            s.PhoneNumber,
            s.ClassId,
            s.Class?.ClassName ?? string.Empty,
            s.Grades?.Select(g => g.GradeId).ToList() ?? new List<Guid>()))
            .ToList();
    }
}