using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Entities;
using FluentValidation;
using StudentManagementSystem.Domain.Core.Interfaces;
using MediatR;

namespace StudentManagementSystem.Application.Features.Schedules.Write;

public record CreateScheduleCommand(
    DayOfWeek DayOfWeek,
    DateTime StartTime,
    DateTime EndTime,
    Guid ClassId,
    Guid SubjectId,
    Guid TeacherId) : ICommand<CreateScheduleCommandResponse>;

public record CreateScheduleCommandResponse(Guid Id, DayOfWeek Day, TimeSpan StartTime, TimeSpan EndTime);

public class CreateScheduleCommandValidator : AbstractValidator<CreateScheduleCommand>
{
    public CreateScheduleCommandValidator()
    {
        RuleFor(x => x.DayOfWeek).IsInEnum();
        RuleFor(x => x.StartTime).LessThan(x => x.EndTime);
        RuleFor(x => x.ClassId).NotEmpty();
        RuleFor(x => x.SubjectId).NotEmpty();
        RuleFor(x => x.TeacherId).NotEmpty();
    }
}

public class CreateScheduleCommandHandler(IScheduleRepository _repo)
    : ICommandHandler<CreateScheduleCommand, CreateScheduleCommandResponse>
{
    public async Task<CreateScheduleCommandResponse> Handle(
        CreateScheduleCommand request, CancellationToken cancellationToken)
    {
        var schedule = new Schedule
        {
            ScheduleId = Guid.NewGuid(),
            DayOfWeek = request.DayOfWeek,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            ClassId = request.ClassId,
            SubjectId = request.SubjectId,
            TeacherId = request.TeacherId
        };

        await _repo.AddAsync(schedule);

        return new CreateScheduleCommandResponse(
            schedule.ScheduleId,
            schedule.DayOfWeek,
            schedule.StartTime.TimeOfDay,
            schedule.EndTime.TimeOfDay);
    }
}