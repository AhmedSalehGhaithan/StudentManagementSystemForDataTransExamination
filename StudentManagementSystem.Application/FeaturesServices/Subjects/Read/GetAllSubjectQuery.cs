using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Domain.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace StudentManagementSystem.Application.Features.Subjects.Read;

public record GetAllSubjectsQuery : IQuery<ICollection<SubjectDto>>;

public class GetAllSubjectsQueryHandler(ISubjectRepository _repo)
    : IQueryHandler<GetAllSubjectsQuery, ICollection<SubjectDto>>
{
    public async Task<ICollection<SubjectDto>> Handle(
        GetAllSubjectsQuery request,
        CancellationToken cancellationToken)
    {
        var subjects = await _repo.GetAllAsync(q =>
            q.Include(s => s.Teacher)
             .Include(s => s.Grades)
             .Include(s => s.Schedules),
            cancellationToken);

        return subjects.Select(s => new SubjectDto(
            s.SubjectId,
            s.Name,
            s.Description,
            s.TeacherId,
            s.Teacher?.FirstName + " " + s.Teacher?.LastName,
            s.Grades?.Select(g => g.GradeId).ToList() ?? new List<Guid>(),
            s.Schedules?.Select(sc => sc.ScheduleId).ToList() ?? new List<Guid>()))
            .ToList();
    }
}