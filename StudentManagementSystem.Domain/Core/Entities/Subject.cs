namespace StudentManagementSystem.Domain.Core.Entities;

public class Subject
{
    public Guid SubjectId { get; set; }


    public string Name { get; set; }


    public string Description { get; set; }


    public Guid TeacherId { get; set; }

    public virtual Teacher Teacher { get; set; }
    public virtual ICollection<Grade> Grades { get; set; }
    public virtual ICollection<Schedule> Schedules { get; set; }
}
