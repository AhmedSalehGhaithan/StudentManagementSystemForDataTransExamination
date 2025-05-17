using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace StudentManagementSystem.Application.Features.Classes.Write;

public record UpdateClassCommand(
    Guid ClassId,
    string ClassName,
    string Section,
    string RoomNumber,
    Guid TeacherId) : ICommand<Guid>;

public class UpdateClassCommandValidator : AbstractValidator<UpdateClassCommand>
{
    public UpdateClassCommandValidator()
    {
        RuleFor(x => x.ClassId).NotEmpty();
        RuleFor(x => x.ClassName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Section).MaximumLength(10);
        RuleFor(x => x.RoomNumber).MaximumLength(20);
        RuleFor(x => x.TeacherId).NotEmpty();
    }
}

public class UpdateClassCommandHandler(IClassRepository _repo)
    : ICommandHandler<UpdateClassCommand, Guid>
{
    public async Task<Guid> Handle(
        UpdateClassCommand request, CancellationToken cancellationToken)
    {
        var classEntity = await _repo.GetByIdAsync(request.ClassId, cancellationToken);
        if (classEntity == null)
            return Guid.Empty;

        classEntity.ClassName = request.ClassName;
        classEntity.Section = request.Section;
        classEntity.RoomNumber = request.RoomNumber;
        classEntity.TeacherId = request.TeacherId;

        await _repo.UpdateAsync(classEntity);
        return classEntity.ClassId;
    }
}