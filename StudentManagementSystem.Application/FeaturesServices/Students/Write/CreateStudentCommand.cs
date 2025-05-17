using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Entities;
using FluentValidation;
using StudentManagementSystem.Domain.Core.Interfaces;

namespace StudentManagementSystem.Application.FeaturesServices.Students.Write;

public record CreateStudentCommand(
    string FirstName,
    string LastName,
    string PhoneNumber,
    Guid ClassId) : ICommand<CreateStudentCommandResponse>;

public record CreateStudentCommandResponse(Guid Id, string FullName, Guid ClassId);

public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.ClassId).NotEmpty();
    }
}

public class CreateStudentCommandHandler(IStudentRepository _repo)
    : ICommandHandler<CreateStudentCommand, CreateStudentCommandResponse>
{
    public async Task<CreateStudentCommandResponse> Handle(
        CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = new Student
        {
            StudentId = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            ClassId = request.ClassId
        };

        await _repo.AddAsync(student);

        return new CreateStudentCommandResponse(
            student.StudentId,
            $"{student.FirstName} {student.LastName}",
            student.ClassId);
    }
}