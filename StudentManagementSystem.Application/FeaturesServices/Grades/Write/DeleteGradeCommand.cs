using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace StudentManagementSystem.Application.Features.Grades.Write;

public record DeleteGradeCommand(Guid Id) : ICommand<Guid>;

public class DeleteGradeCommandValidator : AbstractValidator<DeleteGradeCommand>
{
    public DeleteGradeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Grade ID is required");
    }
}

public class DeleteGradeCommandHandler(IGradeRepository _repo)
    : ICommandHandler<DeleteGradeCommand, Guid>
{
    public async Task<Guid> Handle(
        DeleteGradeCommand request, CancellationToken cancellationToken)
    {
        var grade = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (grade == null)
            return Guid.Empty;

        await _repo.DeleteAsync(grade);
        return request.Id;
    }
}