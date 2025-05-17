using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace StudentManagementSystem.Application.Features.Students.Write;

public record DeleteStudentCommand(Guid Id) : ICommand<Guid>;

public class DeleteStudentCommandValidator : AbstractValidator<DeleteStudentCommand>
{
    public DeleteStudentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Student ID is required");
    }
}

public class DeleteStudentCommandHandler(IStudentRepository _repo)
    : ICommandHandler<DeleteStudentCommand, Guid>
{
    public async Task<Guid> Handle(
        DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (student == null)
            return Guid.Empty;

        await _repo.DeleteAsync(student);
        return request.Id;
    }
}