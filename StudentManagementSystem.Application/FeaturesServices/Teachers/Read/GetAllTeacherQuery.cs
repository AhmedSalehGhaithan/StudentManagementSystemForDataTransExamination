using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Domain.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace StudentManagementSystem.Application.Features.Teachers.Read;

public record GetAllTeachersQuery : IQuery<ICollection<TeacherDto>>;

public class GetAllTeachersQueryHandler(ITeacherRepository _repo)
    : IQueryHandler<GetAllTeachersQuery, ICollection<TeacherDto>>
{
    public async Task<ICollection<TeacherDto>> Handle(
        GetAllTeachersQuery request,
        CancellationToken cancellationToken)
    {
        var teachers = await _repo.GetAllAsync(q =>
            q.Include(t => t.Subjects)
             .Include(t => t.Classes)
             .Include(t => t.Schedules),
            cancellationToken);

        return teachers.Select(t => new TeacherDto(
            t.TeacherId,
            t.FirstName,
            t.LastName,
            t.PhoneNumber,
            t.Subjects?.Select(s => s.SubjectId).ToList() ?? new List<Guid>(),
            t.Classes?.Select(c => c.ClassId).ToList() ?? new List<Guid>(),
            t.Schedules?.Select(sc => sc.ScheduleId).ToList() ?? new List<Guid>()))
            .ToList();
    }
}