using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Domain.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace StudentManagementSystem.Application.Features.Classes.Read;

public record GetClassByIdQuery(Guid Id) : IQuery<ClassDto>;

public class GetClassByIdQueryValidator : AbstractValidator<GetClassByIdQuery>
{
    public GetClassByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Class Id is required!");
    }
}

public class GetClassByIdQueryHandler(IClassRepository _repo)
    : IRequestHandler<GetClassByIdQuery, ClassDto>
{
    public async Task<ClassDto> Handle(
        GetClassByIdQuery request, CancellationToken cancellationToken)
    {
        var classEntity = await _repo.GetByIdAsync(request.Id, cancellationToken);

        return new ClassDto(
            classEntity!.ClassId,
            classEntity.ClassName,
            classEntity.Section,
            classEntity.RoomNumber,
            classEntity.TeacherId,
            classEntity.Teacher?.FirstName + " " + classEntity.Teacher?.LastName,
            classEntity.Students?.Select(s => s.StudentId).ToList() ?? new List<Guid>(),
            classEntity.Schedules?.Select(sc => sc.ScheduleId).ToList() ?? new List<Guid>());
    }
}