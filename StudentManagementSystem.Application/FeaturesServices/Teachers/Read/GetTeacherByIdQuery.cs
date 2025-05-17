using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace StudentManagementSystem.Application.Features.Teachers.Read;

public record GetTeacherByIdQuery(Guid Id) : IQuery<TeacherDto>;

public class GetTeacherByIdQueryValidator : AbstractValidator<GetTeacherByIdQuery>
{
    public GetTeacherByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Teacher Id is required!");
    }
}

public class GetTeacherByIdQueryHandler(ITeacherRepository _repo)
    : IQueryHandler<GetTeacherByIdQuery, TeacherDto>
{
    public async Task<TeacherDto> Handle(
        GetTeacherByIdQuery request, CancellationToken cancellationToken)
    {
        var teacher = await _repo.GetByIdAsync(request.Id, cancellationToken);

        return new TeacherDto(
            teacher!.TeacherId,
            teacher.FirstName,
            teacher.LastName,
            teacher.PhoneNumber,
            teacher.Subjects?.Select(s => s.SubjectId).ToList() ?? new List<Guid>(),
            teacher.Classes?.Select(c => c.ClassId).ToList() ?? new List<Guid>(),
            teacher.Schedules?.Select(sc => sc.ScheduleId).ToList() ?? new List<Guid>());
    }
}