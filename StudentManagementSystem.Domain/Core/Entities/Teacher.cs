namespace StudentManagementSystem.Domain.Core.Entities;

public class Teacher
{
    public Guid TeacherId { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string PhoneNumber { get; set; }
    public virtual ICollection<Subject> Subjects { get; set; }
    public virtual ICollection<Class> Classes { get; set; }
    public virtual ICollection<Schedule> Schedules { get; set; }
}
