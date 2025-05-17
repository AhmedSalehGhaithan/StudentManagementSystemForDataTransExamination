using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Domain.Core.Entities;
using StudentManagementSystem.Domain.Core.Interfaces;
using StudentManagementSystem.Infrastructre.Data;
using StudentManagementSystem.Infrastructre.Repositories;

public class StudentRepository(ApplicationDbContext _context)
    : BaseRepository<Student>(_context), IStudentRepository
{
    public override async Task<Student?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        return await _context.Students
            .Include(s => s.Class)
            .ThenInclude(c => c.Teacher)
            .Include(s => s.Grades)
            .ThenInclude(g => g.Subject)
            .FirstOrDefaultAsync(s => s.StudentId == Id, cancellationToken);
    }

    // for custoization override the method needed
}

public class TeacherRepository(ApplicationDbContext _context)
    : BaseRepository<Teacher>(_context), ITeacherRepository
{
    public override async Task<Teacher?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        return await _context.Teachers
            .Include(t => t.Subjects)
            .Include(t => t.Classes)
            .Include(t => t.Schedules)
            .ThenInclude(s => s.Class)
            .Include(t => t.Schedules)
            .ThenInclude(s => s.Subject)
            .FirstOrDefaultAsync(t => t.TeacherId == Id, cancellationToken);
    }
}

public class SubjectRepository(ApplicationDbContext _context)
    : BaseRepository<Subject>(_context), ISubjectRepository
{
    public override async Task<Subject?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        return await _context.Subjects
            .Include(s => s.Teacher)
            .Include(s => s.Grades)
            .ThenInclude(g => g.Student)
            .Include(s => s.Schedules)
            .ThenInclude(sc => sc.Class)
            .FirstOrDefaultAsync(s => s.SubjectId == Id, cancellationToken);
    }
}

public class GradeRepository(ApplicationDbContext _context)
    : BaseRepository<Grade>(_context), IGradeRepository
{
    public override async Task<Grade?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        return await _context.Grades
            .Include(g => g.Student)
            .ThenInclude(s => s.Class)
            .Include(g => g.Subject)
            .ThenInclude(s => s.Teacher)
            .FirstOrDefaultAsync(g => g.GradeId == Id, cancellationToken);
    }
}

public class ScheduleRepository(ApplicationDbContext _context)
    : BaseRepository<Schedule>(_context), IScheduleRepository
{
    public override async Task<Schedule?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        return await _context.Schedules
            .Include(s => s.Class)
            .ThenInclude(c => c.Teacher)
            .Include(s => s.Subject)
            .ThenInclude(sub => sub.Teacher)
            .Include(s => s.Teacher)
            .FirstOrDefaultAsync(s => s.ScheduleId == Id, cancellationToken);
    }
}

public class ClassRepository(ApplicationDbContext _context)
    : BaseRepository<Class>(_context), IClassRepository
{
    public override async Task<Class?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        return await _context.Classes
            .Include(c => c.Teacher)
            .Include(c => c.Students)
            .Include(c => c.Schedules)
            .ThenInclude(s => s.Subject)
            .FirstOrDefaultAsync(c => c.ClassId == Id, cancellationToken);
    }
}