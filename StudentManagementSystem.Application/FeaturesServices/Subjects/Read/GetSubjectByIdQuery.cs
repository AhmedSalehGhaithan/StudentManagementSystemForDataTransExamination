using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;

namespace StudentManagementSystem.Application.Features.Subjects.Read;

public record GetSubjectByIdQuery(Guid Id) : IQuery<SubjectDto>;

public class GetSubjectByIdQueryValidator : AbstractValidator<GetSubjectByIdQuery>
{
    public GetSubjectByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Subject Id is required!");
    }
}

public class GetSubjectByIdQueryHandler(ISubjectRepository _repo)
    : IQueryHandler<GetSubjectByIdQuery, SubjectDto>
{
    public async Task<SubjectDto> Handle(
        GetSubjectByIdQuery request, CancellationToken cancellationToken)
    {
        var subject = await _repo.GetByIdAsync(request.Id, cancellationToken);

        return new SubjectDto(
            subject!.SubjectId,
            subject.Name,
            subject.Description,
            subject.TeacherId,
            subject.Teacher?.FirstName + " " + subject.Teacher?.LastName,
            subject.Grades?.Select(g => g.GradeId).ToList() ?? new List<Guid>(),
            subject.Schedules?.Select(sc => sc.ScheduleId).ToList() ?? new List<Guid>());
    }
}