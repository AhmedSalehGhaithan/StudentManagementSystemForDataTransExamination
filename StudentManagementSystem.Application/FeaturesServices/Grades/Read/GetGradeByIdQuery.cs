using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace StudentManagementSystem.Application.Features.Grades.Read;

public record GetGradeByIdQuery(Guid Id) : IQuery<GradeDto>;

public class GetGradeByIdQueryValidator : AbstractValidator<GetGradeByIdQuery>
{
    public GetGradeByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Grade Id is required!");
    }
}

public class GetGradeByIdQueryHandler(IGradeRepository _repo)
    : IQueryHandler<GetGradeByIdQuery, GradeDto>
{
    public async Task<GradeDto> Handle(
        GetGradeByIdQuery request, CancellationToken cancellationToken)
    {
        var grade = await _repo.GetByIdAsync(request.Id, cancellationToken);

        return new GradeDto(
            grade!.GradeId,
            grade.Score,
            grade.StudentId,
            grade.Student?.FirstName + " " + grade.Student?.LastName,
            grade.SubjectId,
            grade.Subject?.Name,
            grade.Subject?.Teacher?.FirstName + " " + grade.Subject?.Teacher?.LastName);
    }
}