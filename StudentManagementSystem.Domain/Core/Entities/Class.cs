namespace StudentManagementSystem.Domain.Core.Entities;

public class Class
{
    public Guid ClassId { get; set; }


    public string ClassName { get; set; }


    public string Section { get; set; }

    public string RoomNumber { get; set; }


    public Guid TeacherId { get; set; }

    public virtual Teacher Teacher { get; set; }
    public virtual ICollection<Student> Students { get; set; }
    public virtual ICollection<Schedule> Schedules { get; set; }
}
