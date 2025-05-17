using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace StudentManagementSystem.Application.Features.Grades.Write;

public record UpdateGradeCommand(
    Guid GradeId,
    decimal Score,
    Guid StudentId,
    Guid SubjectId) : ICommand<Guid>;

public class UpdateGradeCommandValidator : AbstractValidator<UpdateGradeCommand>
{
    public UpdateGradeCommandValidator()
    {
        RuleFor(x => x.GradeId).NotEmpty();
        RuleFor(x => x.Score).InclusiveBetween(0, 100);
        RuleFor(x => x.StudentId).NotEmpty();
        RuleFor(x => x.SubjectId).NotEmpty();
    }
}

public class UpdateGradeCommandHandler(IGradeRepository _repo) 
    : ICommandHandler<UpdateGradeCommand, Guid>
{
    public async Task<Guid> Handle(
        UpdateGradeCommand request, CancellationToken cancellationToken)
    {
        var grade = await _repo.GetByIdAsync(request.GradeId, cancellationToken);
        if (grade == null)
            return Guid.Empty;

        grade.Score = request.Score;
        grade.StudentId = request.StudentId;
        grade.SubjectId = request.SubjectId;

        await _repo.UpdateAsync(grade);
        return grade.GradeId;
    }
}