using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace StudentManagementSystem.Application.Features.Teachers.Write;

public record UpdateTeacherCommand(
    Guid TeacherId,
    string FirstName,
    string LastName,
    string PhoneNumber) : ICommand<Guid>;

public class UpdateTeacherCommandValidator : AbstractValidator<UpdateTeacherCommand>
{
    public UpdateTeacherCommandValidator()
    {
        RuleFor(x => x.TeacherId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20);
    }
}

public class UpdateTeacherCommandHandler(ITeacherRepository _repo)
    : ICommandHandler<UpdateTeacherCommand, Guid>
{
    public async Task<Guid> Handle(
        UpdateTeacherCommand request, CancellationToken cancellationToken)
    {
        var teacher = await _repo.GetByIdAsync(request.TeacherId, cancellationToken);
        if (teacher == null)
            return Guid.Empty;

        teacher.FirstName = request.FirstName;
        teacher.LastName = request.LastName;
        teacher.PhoneNumber = request.PhoneNumber;

        await _repo.UpdateAsync(teacher);
        return teacher.TeacherId;
    }
}