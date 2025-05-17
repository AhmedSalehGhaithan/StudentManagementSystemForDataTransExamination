using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace StudentManagementSystem.Application.Features.Schedules.Write;

public record DeleteScheduleCommand(Guid Id) : ICommand<Guid>;

public class DeleteScheduleCommandValidator : AbstractValidator<DeleteScheduleCommand>
{
    public DeleteScheduleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Schedule ID is required");
    }
}

public class DeleteScheduleCommandHandler(IScheduleRepository _repo)
    : ICommandHandler<DeleteScheduleCommand, Guid>
{
    public async Task<Guid> Handle(
        DeleteScheduleCommand request, CancellationToken cancellationToken)
    {
        var schedule = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (schedule == null)
            return Guid.Empty;

        await _repo.DeleteAsync(schedule);
        return request.Id;
    }
}