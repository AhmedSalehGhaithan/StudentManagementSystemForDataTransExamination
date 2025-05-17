using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Entities;
using FluentValidation;
using StudentManagementSystem.Domain.Core.Interfaces;
using MediatR;

namespace StudentManagementSystem.Application.Features.Classes.Write;

public record CreateClassCommand(
    string ClassName,
    string Section,
    string RoomNumber,
    Guid TeacherId) : ICommand<CreateClassCommandResponse>;

public record CreateClassCommandResponse(Guid Id, string ClassName, Guid TeacherId);

public class CreateClassCommandValidator : AbstractValidator<CreateClassCommand>
{
    public CreateClassCommandValidator()
    {
        RuleFor(x => x.ClassName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Section).MaximumLength(10);
        RuleFor(x => x.RoomNumber).MaximumLength(20);
        RuleFor(x => x.TeacherId).NotEmpty();
    }
}

public class CreateClassCommandHandler(IClassRepository _repo)
    : ICommandHandler<CreateClassCommand, CreateClassCommandResponse>
{
    public async Task<CreateClassCommandResponse> Handle(
        CreateClassCommand request, CancellationToken cancellationToken)
    {
        var classEntity = new Class
        {
            ClassId = Guid.NewGuid(),
            ClassName = request.ClassName,
            Section = request.Section,
            RoomNumber = request.RoomNumber,
            TeacherId = request.TeacherId
        };

        await _repo.AddAsync(classEntity);

        return new CreateClassCommandResponse(
            classEntity.ClassId,
            classEntity.ClassName,
            classEntity.TeacherId);
    }
}