using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace StudentManagementSystem.Application.Features.Subjects.Write;

public record UpdateSubjectCommand(
    Guid SubjectId,
    string Name,
    string Description,
    Guid TeacherId) : ICommand<Guid>;

public class UpdateSubjectCommandValidator : AbstractValidator<UpdateSubjectCommand>
{
    public UpdateSubjectCommandValidator()
    {
        RuleFor(x => x.SubjectId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.TeacherId).NotEmpty();
    }
}

public class UpdateSubjectCommandHandler(ISubjectRepository _repo)
    : ICommandHandler<UpdateSubjectCommand, Guid>
{
    public async Task<Guid> Handle(
        UpdateSubjectCommand request, CancellationToken cancellationToken)
    {
        var subject = await _repo.GetByIdAsync(request.SubjectId, cancellationToken);
        if (subject == null)
            return Guid.Empty;

        subject.Name = request.Name;
        subject.Description = request.Description;
        subject.TeacherId = request.TeacherId;

        await _repo.UpdateAsync(subject);
        return subject.SubjectId;
    }
}