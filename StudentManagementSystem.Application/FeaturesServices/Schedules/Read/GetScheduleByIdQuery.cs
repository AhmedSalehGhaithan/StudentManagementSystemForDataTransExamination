using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace StudentManagementSystem.Application.Features.Schedules.Read;

public record GetScheduleByIdQuery(Guid Id) : IQuery<ScheduleDto>;

public class GetScheduleByIdQueryValidator : AbstractValidator<GetScheduleByIdQuery>
{
    public GetScheduleByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Schedule Id is required!");
    }
}

public class GetScheduleByIdQueryHandler(IScheduleRepository _repo)
    : IQueryHandler<GetScheduleByIdQuery, ScheduleDto>
{
    public async Task<ScheduleDto> Handle(
        GetScheduleByIdQuery request, CancellationToken cancellationToken)
    {
        var schedule = await _repo.GetByIdAsync(request.Id, cancellationToken);

        return new ScheduleDto(
            schedule!.ScheduleId,
            schedule.DayOfWeek,
            schedule.StartTime,
            schedule.EndTime,
            schedule.ClassId,
            schedule.Class?.ClassName ?? string.Empty,
            schedule.SubjectId,
            schedule.Subject?.Name ?? string.Empty,
            schedule.TeacherId,
            schedule.Teacher?.FirstName + " " + schedule.Teacher?.LastName);
    }
}