using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace StudentManagementSystem.Application.Features.Teachers.Write;

public record DeleteTeacherCommand(Guid Id) : ICommand<Guid>;

public class DeleteTeacherCommandValidator : AbstractValidator<DeleteTeacherCommand>
{
    public DeleteTeacherCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Teacher ID is required");
    }
}

public class DeleteTeacherCommandHandler(ITeacherRepository _repo)
    : ICommandHandler<DeleteTeacherCommand, Guid>
{
    public async Task<Guid> Handle(
        DeleteTeacherCommand request, CancellationToken cancellationToken)
    {
        var teacher = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (teacher == null)
            return Guid.Empty;

        await _repo.DeleteAsync(teacher);
        return request.Id;
    }
}