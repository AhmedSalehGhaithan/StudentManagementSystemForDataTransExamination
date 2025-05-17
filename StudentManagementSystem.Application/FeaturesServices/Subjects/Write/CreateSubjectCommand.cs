using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Entities;
using StudentManagementSystem.Domain.Core.Interfaces;
using MediatR;
using FluentValidation;

namespace StudentManagementSystem.Application.Features.Subjects.Write;

public record CreateSubjectCommand(
    string Name,
    string Description,
    Guid TeacherId) : ICommand<CreateSubjectCommandResponse>;

public record CreateSubjectCommandResponse(Guid Id, string Name, Guid TeacherId);

public class CreateSubjectCommandValidator : AbstractValidator<CreateSubjectCommand>
{
    public CreateSubjectCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.TeacherId).NotEmpty();
    }
}

public class CreateSubjectCommandHandler(ISubjectRepository _repo)
    : ICommandHandler<CreateSubjectCommand, CreateSubjectCommandResponse>
{
    public async Task<CreateSubjectCommandResponse> Handle(
        CreateSubjectCommand request, CancellationToken cancellationToken)
    {
        var subject = new Subject
        {
            SubjectId = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            TeacherId = request.TeacherId
        };

        await _repo.AddAsync(subject);

        return new CreateSubjectCommandResponse(
            subject.SubjectId,
            subject.Name,
            subject.TeacherId);
    }
}