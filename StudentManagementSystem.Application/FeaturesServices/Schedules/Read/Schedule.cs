using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Domain.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace StudentManagementSystem.Application.Features.Schedules.Read;

public record GetAllSchedulesQuery : IQuery<ICollection<ScheduleDto>>;

public class GetAllSchedulesQueryHandler(IScheduleRepository _repo)
    : IQueryHandler<GetAllSchedulesQuery, ICollection<ScheduleDto>>
{
    public async Task<ICollection<ScheduleDto>> Handle(
        GetAllSchedulesQuery request,
        CancellationToken cancellationToken)
    {
        var schedules = await _repo.GetAllAsync(q =>
            q.Include(s => s.Class)
             .Include(s => s.Subject)
             .Include(s => s.Teacher),
            cancellationToken);

        return schedules.Select(s => new ScheduleDto(
            s.ScheduleId,
            s.DayOfWeek,
            s.StartTime,
            s.EndTime,
            s.ClassId,
            s.Class?.ClassName!,
            s.SubjectId,
            s.Subject?.Name!,
            s.TeacherId,
            s.Teacher?.FirstName + " " + s.Teacher?.LastName))
            .ToList();
    }
}