using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Domain.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace StudentManagementSystem.Application.Features.Classes.Read;

public record GetAllClassesQuery : IQuery<ICollection<ClassDto>>;

public class GetAllClassesQueryHandler(IClassRepository _repo)
    : IQueryHandler<GetAllClassesQuery, ICollection<ClassDto>>
{
    public async Task<ICollection<ClassDto>> Handle(
        GetAllClassesQuery request,
        CancellationToken cancellationToken)
    {
        var classes = await _repo.GetAllAsync(q =>
            q.Include(c => c.Teacher)
             .Include(c => c.Students)
             .Include(c => c.Schedules),
            cancellationToken);

        return classes.Select(c => new ClassDto(
            c.ClassId,
            c.ClassName,
            c.Section,
            c.RoomNumber,
            c.TeacherId,
            c.Teacher?.FirstName + " " + c.Teacher?.LastName,
            c.Students?.Select(s => s.StudentId).ToList() ?? new List<Guid>(),
            c.Schedules?.Select(sc => sc.ScheduleId).ToList() ?? new List<Guid>()))
            .ToList();
    }
}