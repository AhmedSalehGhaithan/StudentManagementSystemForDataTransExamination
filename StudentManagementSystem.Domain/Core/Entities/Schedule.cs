using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Domain.Core.Entities;

public class Schedule
{
    public Guid ScheduleId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }


    public DateTime StartTime { get; set; }

    [DataType(DataType.Time)]
    public DateTime EndTime { get; set; }

    // Foreign keys
    public Guid ClassId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid TeacherId { get; set; }

    // Navigation properties
    public virtual Class Class { get; set; }
    public virtual Subject Subject { get; set; }
    public virtual Teacher Teacher { get; set; }
}