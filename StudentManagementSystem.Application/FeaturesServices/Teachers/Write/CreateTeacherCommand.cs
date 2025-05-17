using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Entities;
using FluentValidation;
using StudentManagementSystem.Domain.Core.Interfaces;
using MediatR;

namespace StudentManagementSystem.Application.Features.Teachers.Write;

public record CreateTeacherCommand(
    string FirstName,
    string LastName,
    string PhoneNumber) : ICommand<CreateTeacherCommandResponse>;

public record CreateTeacherCommandResponse(Guid Id, string FullName);

public class CreateTeacherCommandValidator : AbstractValidator<CreateTeacherCommand>
{
    public CreateTeacherCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20);
    }
}

public class CreateTeacherCommandHandler(ITeacherRepository _repo)
    : ICommandHandler<CreateTeacherCommand, CreateTeacherCommandResponse>
{
    public async Task<CreateTeacherCommandResponse> Handle(
        CreateTeacherCommand request, CancellationToken cancellationToken)
    {
        var teacher = new Teacher
        {
            TeacherId = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber
        };

        await _repo.AddAsync(teacher);

        return new CreateTeacherCommandResponse(
            teacher.TeacherId,
            $"{teacher.FirstName} {teacher.LastName}");
    }
}