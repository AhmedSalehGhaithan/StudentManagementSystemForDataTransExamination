using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;

namespace StudentManagementSystem.Application.Features.Classes.Write;

public record DeleteClassCommand(Guid Id) : ICommand<Guid>;

public class DeleteClassCommandValidator : AbstractValidator<DeleteClassCommand>
{
    public DeleteClassCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Class ID is required");
    }
}

public class DeleteClassCommandHandler(IClassRepository _repo)
    : ICommandHandler<DeleteClassCommand, Guid>
{
    public async Task<Guid> Handle(
        DeleteClassCommand request, CancellationToken cancellationToken)
    {
        var classEntity = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (classEntity == null)
            return Guid.Empty;

        await _repo.DeleteAsync(classEntity);
        return request.Id;
    }
}