namespace StudentManagementSystem.Application.Dtos;
public record StudentDto(
    Guid StudentId,
    string FirstName,
    string LastName,
    string PhoneNumber,
    Guid ClassId,
    string ClassName,
    List<Guid> GradeIds);

public record TeacherDto(
    Guid TeacherId,
    string FirstName,
    string LastName,
    string PhoneNumber,
    List<Guid> SubjectIds,
    List<Guid> ClassIds,
    List<Guid> ScheduleIds);

public record SubjectDto(
    Guid SubjectId,
    string Name,
    string Description,
    Guid TeacherId,
    string TeacherName,
    List<Guid> GradeIds,
    List<Guid> ScheduleIds);


public record ClassDto(
    Guid ClassId,
    string ClassName,
    string Section,
    string RoomNumber,
    Guid TeacherId,
    string TeacherName,
    List<Guid> StudentIds,
    List<Guid> ScheduleIds);
public record GradeDto(
    Guid GradeId,
    decimal Score,
    Guid StudentId,
    string StudentName,
    Guid SubjectId,
    string SubjectName,
    string TeacherName);



public record ScheduleDto(
    Guid ScheduleId,
    DayOfWeek DayOfWeek,
    DateTime StartTime,
    DateTime EndTime,
    Guid ClassId,
    string ClassName,
    Guid SubjectId,
    string SubjectName,
    Guid TeacherId,
    string TeacherName);
