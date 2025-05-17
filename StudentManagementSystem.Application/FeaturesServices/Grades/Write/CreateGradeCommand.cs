using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Entities;
using FluentValidation;
using StudentManagementSystem.Domain.Core.Interfaces;
using MediatR;

namespace StudentManagementSystem.Application.Features.Grades.Write;

public record CreateGradeCommand(
    decimal Score,
    Guid StudentId,
    Guid SubjectId) : ICommand<CreateGradeCommandResponse>;

public record CreateGradeCommandResponse(Guid Id, decimal Score, Guid StudentId, Guid SubjectId);

public class CreateGradeCommandValidator : AbstractValidator<CreateGradeCommand>
{
    public CreateGradeCommandValidator()
    {
        RuleFor(x => x.Score).InclusiveBetween(0, 100);
        RuleFor(x => x.StudentId).NotEmpty();
        RuleFor(x => x.SubjectId).NotEmpty();
    }
}

public class CreateGradeCommandHandler(IGradeRepository _repo)
    : ICommandHandler<CreateGradeCommand, CreateGradeCommandResponse>
{
    public async Task<CreateGradeCommandResponse> Handle(
        CreateGradeCommand request, CancellationToken cancellationToken)
    {
        var grade = new Grade
        {
            GradeId = Guid.NewGuid(),
            Score = request.Score,
            StudentId = request.StudentId,
            SubjectId = request.SubjectId
        };

        await _repo.AddAsync(grade);

        return new CreateGradeCommandResponse(
            grade.GradeId,
            grade.Score,
            grade.StudentId,
            grade.SubjectId);
    }
}