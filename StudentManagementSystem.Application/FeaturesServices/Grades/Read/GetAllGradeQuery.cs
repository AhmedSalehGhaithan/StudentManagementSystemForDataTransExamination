using StudentManagementSystem.Application.Abstractions.CQRS;
using StudentManagementSystem.Application.Dtos;
using StudentManagementSystem.Domain.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace StudentManagementSystem.Application.Features.Grades.Read;

public record GetAllGradesQuery : IQuery<ICollection<GradeDto>>;

public class GetAllGradesQueryHandler(IGradeRepository _repo)
    : IQueryHandler<GetAllGradesQuery, ICollection<GradeDto>>
{
    public async Task<ICollection<GradeDto>> Handle(
        GetAllGradesQuery request,
        CancellationToken cancellationToken)
    {
        var grades = await _repo.GetAllAsync(q =>
            q.Include(g => g.Student)
             .Include(g => g.Subject),
            cancellationToken);

        return grades.Select(g => new GradeDto(
            g.GradeId,
            g.Score,
            g.StudentId,
            g.Student?.FirstName + " " + g.Student?.LastName,
            g.SubjectId,
            g.Subject?.Name!,
            g.Subject?.Teacher?.FirstName + " " + g.Subject?.Teacher?.LastName))
            .ToList();
    }
}