using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace StudentManagementSystem.Application.Features.Students.Write;

public record UpdateStudentCommand(
    Guid StudentId,
    string FirstName,
    string LastName,
    string PhoneNumber,
    Guid ClassId) : ICommand<Guid>;

public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentCommandValidator()
    {
        RuleFor(x => x.StudentId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.ClassId).NotEmpty();
    }
}

public class UpdateStudentCommandHandler(IStudentRepository _repo)
    : ICommandHandler<UpdateStudentCommand, Guid>
{
    public async Task<Guid> Handle(
        UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _repo.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
            return Guid.Empty;

        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        student.PhoneNumber = request.PhoneNumber;
        student.ClassId = request.ClassId;

        await _repo.UpdateAsync(student);
        return student.StudentId;
    }
}