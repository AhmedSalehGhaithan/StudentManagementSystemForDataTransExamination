using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace StudentManagementSystem.Application.Features.Schedules.Write;

public record UpdateScheduleCommand(
    Guid ScheduleId,
    DayOfWeek DayOfWeek,
    DateTime StartTime,
    DateTime EndTime,
    Guid ClassId,
    Guid SubjectId,
    Guid TeacherId) : ICommand<Guid>;

public class UpdateScheduleCommandValidator : AbstractValidator<UpdateScheduleCommand>
{
    public UpdateScheduleCommandValidator()
    {
        RuleFor(x => x.ScheduleId).NotEmpty();
        RuleFor(x => x.DayOfWeek).IsInEnum();
        RuleFor(x => x.StartTime).LessThan(x => x.EndTime);
        RuleFor(x => x.ClassId).NotEmpty();
        RuleFor(x => x.SubjectId).NotEmpty();
        RuleFor(x => x.TeacherId).NotEmpty();
    }
}

public class UpdateScheduleCommandHandler(IScheduleRepository _repo)
    : ICommandHandler<UpdateScheduleCommand, Guid>
{
    public async Task<Guid> Handle(
        UpdateScheduleCommand request, CancellationToken cancellationToken)
    {
        var schedule = await _repo.GetByIdAsync(request.ScheduleId, cancellationToken);
        if (schedule == null)
            return Guid.Empty;

        schedule.DayOfWeek = request.DayOfWeek;
        schedule.StartTime = request.StartTime;
        schedule.EndTime = request.EndTime;
        schedule.ClassId = request.ClassId;
        schedule.SubjectId = request.SubjectId;
        schedule.TeacherId = request.TeacherId;

        await _repo.UpdateAsync(schedule);
        return schedule.ScheduleId;
    }
}