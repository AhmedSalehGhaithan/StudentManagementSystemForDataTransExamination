using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace StudentManagementSystem.Application.Features.Students.Read;

public record GetStudentByIdQuery(Guid Id) : IQuery<StudentDto>;

public class GetStudentByIdQueryValidator : AbstractValidator<GetStudentByIdQuery>
{
    public GetStudentByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Student Id is required!");
    }
}

public class GetStudentByIdQueryHandler(IStudentRepository _repo)
    : IQueryHandler<GetStudentByIdQuery, StudentDto>
{
    public async Task<StudentDto> Handle(
        GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        var student = await _repo.GetByIdAsync(request.Id, cancellationToken);

        return new StudentDto(
            student!.StudentId,
            student.FirstName,
            student.LastName,
            student.PhoneNumber,
            student.ClassId,
            student.Class?.ClassName ?? string.Empty,
            student.Grades?.Select(g => g.GradeId).ToList() ?? new List<Guid>());
    }
}