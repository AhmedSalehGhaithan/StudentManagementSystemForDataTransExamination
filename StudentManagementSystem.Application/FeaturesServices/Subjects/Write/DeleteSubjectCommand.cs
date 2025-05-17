using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace StudentManagementSystem.Application.Features.Subjects.Write;

public record DeleteSubjectCommand(Guid Id) : ICommand<Guid>;

public class DeleteSubjectCommandValidator : AbstractValidator<DeleteSubjectCommand>
{
    public DeleteSubjectCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Subject ID is required");
    }
}

public class DeleteSubjectCommandHandler(ISubjectRepository _repo)
    : ICommandHandler<DeleteSubjectCommand, Guid>
{
    public async Task<Guid> Handle(
        DeleteSubjectCommand request, CancellationToken cancellationToken)
    {
        var subject = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (subject == null)
            return Guid.Empty;

        await _repo.DeleteAsync(subject);
        return request.Id;
    }
}